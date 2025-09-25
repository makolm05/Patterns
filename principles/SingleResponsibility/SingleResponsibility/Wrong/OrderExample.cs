namespace SingleResponsibility.Wrong
{
    public class Order
    {
        public int ID { get; set; }
        public List<string> Products { get; set; } = new List<string>();
        public decimal TotalPrice { get; set; }

        public void CalculateTotalPrice()
        {
            decimal sum = 0;
            foreach (var product in Products)
            {
                sum += product.Length * 10;
            }
            TotalPrice = sum;
        }

        public void SaveToDatabase()
        {
            Console.WriteLine($"Order {ID} saved to database with price {TotalPrice}");
        }

        public void SendNotification(string email)
        {
            Console.WriteLine($"Email sent to {email}: Your order #{ID} is confirmed!");
        }
    }
}
