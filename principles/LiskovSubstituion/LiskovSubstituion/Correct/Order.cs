namespace LiskovSubstituion.Correct
{
    public abstract class Order
    {
        public List<string> Products { get; private set; } = new();
        public decimal TotalPrice { get; protected set; } = 0;

        public abstract void AddProduct(string product, decimal price);

    }

    public class StandardOrder : Order
    {
        public override void AddProduct(string product, decimal price)
        {
            if (string.IsNullOrWhiteSpace(product))
                throw new ArgumentException("Product name cannot be empty.");
            if (price < 0)
                throw new ArgumentException("Price cannot be negative."); 
            Products.Add(product);
            TotalPrice += price; 
        }
    }

    public class DiscountedOrder : Order
    {
        public decimal Discount { get; }
        public DiscountedOrder(decimal discount)
        {
            Discount = discount;
        }
        public override void AddProduct(string product, decimal price)
        {
            if (string.IsNullOrWhiteSpace(product))
                throw new ArgumentException("Product name cannot be empty.");
            if (price < 0)
                throw new ArgumentException("Price cannot be negative."); 

            Products.Add(product);
            
            TotalPrice += Math.Max(0, price - Discount);
        }
    }
}
