namespace LiskovSubstituion.Wrong
{
    public class Order2
    {
        public decimal Subtotal { get; }
        public string CustomerEmail { get; }
        public string Country { get; }

        public Order2(decimal subtotal, string email, string country)
        {
            Subtotal = subtotal;
            CustomerEmail = email;
            Country = country;
        }
    }

    public class CheckoutService
    {
        // Делает сразу всё → нарушает SRP
        public decimal Checkout(Order2 order, string discountType)
        {
            // 1) Считаем скидку (жёсткие if/else → нарушает OCP)
            decimal discount = 0m;
            if (discountType == "None")
            {
                discount = 0m;
            }
            else if (discountType == "Percent10")
            {
                discount = order.Subtotal * 0.10m;
            }
            else if (discountType == "BlackFriday")
            {
                discount = order.Subtotal >= 100 ? 30 : 10;
            }
            else if (discountType == "Student")
            {
                // Иногда «не поддерживается» → нарушает LSP в будущем,
                // если это будет реализацией общего контракта
                throw new NotSupportedException("Student discount not implemented.");
            }

            var afterDiscount = order.Subtotal - discount;

            // 2) Налог (жёсткая логика внутри → нарушает SRP/OCP)
            decimal tax = 0m;
            if (order.Country == "US") tax = afterDiscount * 0.07m;
            else if (order.Country == "DE") tax = afterDiscount * 0.19m;
            else if (order.Country == "UA") tax = afterDiscount * 0.20m;

            var total = afterDiscount + tax;

            // 3) Уведомление (смешано здесь → нарушает SRP)
            Console.WriteLine($"Email to {order.CustomerEmail}: your total is {total:C}");

            return total;
        }
    }
}
