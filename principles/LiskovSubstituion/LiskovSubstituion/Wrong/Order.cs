namespace LiskovSubstituion.Wrong
{
    public class Order
    {
        public List<string> Products { get; private set; } = new();
        public decimal TotalPrice { get; protected set; } = 0;

        public virtual void AddProduct(string product, decimal price)
        {
            if (string.IsNullOrWhiteSpace(product))
                throw new ArgumentException("Product name cannot be empty.");

            if (price < 0)
                throw new ArgumentException("Price cannot be negative."); // предусловие

            Products.Add(product);
            TotalPrice += price; // постусловие: цена увеличилась
        }
    }

    public class DiscountedOrder : Order
    {
        public decimal Discount { get; }

        public DiscountedOrder(decimal discount)
        {
            Discount = discount;
        }

        // ❌ Нарушение: постусловие ломается
        public override void AddProduct(string product, decimal price)
        {
            // Здесь ослабили предусловие: разрешаем товары с отрицательной ценой
            Products.Add(product);

            // Здесь сломали постусловие: цена уменьшается неправильно
            TotalPrice += price - Discount;
        }
    }
}
