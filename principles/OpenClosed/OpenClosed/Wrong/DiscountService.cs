namespace OpenClosed.Wrong
{
    public class DiscountService
    {
        public decimal ApplyDiscount(string customerType, decimal price)
        {
            if (customerType == "Regular")
                return price;

            if (customerType == "Vip")
                return price * 0.8m;

            if (customerType == "Student")
                return price * 0.5m;

            return price;
        }
    }

}
