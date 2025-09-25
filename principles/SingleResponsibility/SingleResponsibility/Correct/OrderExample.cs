namespace SingleResponsibility.Correct
{
    public interface ICalculator // more abstract level, we can delete it
    {
        public decimal Calculate(int[] array);
    }

    public interface IOrderCalculator : ICalculator
    {
        public decimal CalculateOrderTotalPrice(Order order);
    }

    public interface INotification // more abstract level, we can delete it
    {
        public void SendNotification(string destination, string text);
    }

    public interface IOrderNotification : INotification
    {
        public void SendOrderNotification(string email, Order order);
    }

    public interface IStorage // more abstract level, we can delete it
    {
    }

    public interface IOrderStorage : IStorage
    {
        public void SaveOrder(Order order);
    }

   public class Order
   {
        public int ID { get; set; }
        public decimal TotalPrice { get; set; } 
        public List<string> Products { get; set; } = new List<string>();
   }

    public class OrderStorage : IOrderStorage
    {
        public void SaveOrder(Order order)
        {
            Console.WriteLine($"Order {order.ID} saved to database with price {order.TotalPrice}");
        }
    }

    public class OrderNotificationSender : IOrderNotification
    {
        public void SendNotification(string destination, string text)
        {
            Console.WriteLine($"Email sent to {destination}: {text}");
        }

        public void SendOrderNotification(string email, Order order)
        {
            SendNotification(email, $"Your order #{order.ID} is confirmed!");
        }
    }

    public class OrderCalculator : IOrderCalculator
    {
        public decimal Calculate(int[] array)
        {
            return array.Sum();
        }
        public decimal CalculateOrderTotalPrice(Order order)
        {
            return Calculate(order.Products.Select(x => x.Length * 10).ToArray());
        }
    }
}
