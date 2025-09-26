namespace OpenClosed.Correct
{
    public interface IDiscount
    {
        public decimal GetDiscount(decimal price);
    }

    public class CustomerDiscount : IDiscount
    {
        public decimal GetDiscount(decimal price)
        {
            return price;
        }
    }

    public class VIPDiscount : IDiscount
    {
        public decimal GetDiscount(decimal price)
        {
            return price * 0.8m;
        }
    }

    public class StudentDiscount : IDiscount
    {
        public decimal GetDiscount(decimal price)
        {
            return price * 0.5m;
        }
    }

    public class DiscountService
    {
        public decimal ApplyDiscount(IDiscount discount, decimal price)
        {
            return discount.GetDiscount(price);
        }
    }
}
