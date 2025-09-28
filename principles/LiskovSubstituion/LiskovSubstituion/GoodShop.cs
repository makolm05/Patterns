namespace GoodShop
{
    enum OrderStatuses
    {
        New, Approved, Canceled
    }

    public class Order
    {
        public decimal Subtotal { get; set; }
        public string CustomerEmail { get; set; } = "";
        public string Country { get; set; } = "UA";
        public List<string> Items { get; } = new();
        public decimal Total { get; set; }
        public string Status { get; set; } = OrderStatuses.New.ToString();
        public decimal WeightKg { get; set; }
        public string? Address { get; set; }
    }

    public interface IEmailService
    {
        public void Send(string userEmail, string body);
    }

    public class OrderEmailService : IEmailService
    {
        public void Send(string userEmail, string body)
        {
            Console.WriteLine($"Email with message: {body}, sended to {userEmail}");
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            File.AppendAllText("orders.log", message);
        }
    }

    public interface IOrderValidator
    {
        void Validate(Order order);
    }

    public class OrderValidator : IOrderValidator
    {
        public void Validate(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (order.Subtotal <= 0) throw new ArgumentException("Bad subtotal");
        }
    }

    public interface IDiscount
    {
        public decimal Apply(decimal amount);
    }

    public class NoDiscount : IDiscount
    {
        public decimal Apply(decimal amount) => amount;
    }

    public class StudentDiscount : IDiscount
    {
        public decimal Apply(decimal amount) => amount * 0.9m;
    }

    public class VIPDiscount : IDiscount
    {
        public decimal Apply(decimal amount) => amount * 0.8m;
    }

    public class SuperVIPDiscount : IDiscount
    {
        public decimal Apply(decimal amount) => 0; // 120 percent discount, but it cannot be less than zero
    }

    public interface IPriceCalculator
    {
        public decimal CalculateTotal(Order order, IDiscount discount);
    }

    public abstract class PriceCalculatorBase : IPriceCalculator
    {
        private readonly decimal _deliveryFee;  // инвариант
        protected PriceCalculatorBase(decimal deliveryFee) => _deliveryFee = deliveryFee;

        public decimal CalculateTotal(Order order, IDiscount discount)
        {
            if (order.Subtotal < 0) throw new ArgumentException("Subtotal must be >= 0");

            var discounted = discount.Apply(order.Subtotal);
            var total = discounted + _deliveryFee;

            return total < 0 ? 0 : total;
        }
    }

    public sealed class RegularPriceCalculator : PriceCalculatorBase
    {
        public RegularPriceCalculator() : base(deliveryFee: 50m) { }
    }

    public sealed class ExpressPriceCalculator : PriceCalculatorBase
    {
        public ExpressPriceCalculator() : base(deliveryFee: 100m) { }
    }

    public static class CalculatorSelector
    {
        public static IPriceCalculator Choose(Order o)
            => o.Subtotal >= 50m ? new ExpressPriceCalculator() : new RegularPriceCalculator();
    }

    public class OrderProcessor
    {
        private readonly IOrderValidator _validator;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public OrderProcessor(IOrderValidator validator, ILogger logger, IEmailService emailService)
        {
            _validator = validator;
            _logger = logger;
            _emailService = emailService;
        }

        public void Process(Order order, IDiscount discount)
        {
            _validator.Validate(order);
            order.Total = CalculatorSelector.Choose(order).CalculateTotal(order, discount);

            order.Status = OrderStatuses.Approved.ToString(); // cannot be less than zero

            _logger.Log($"{DateTime.Now}: {order.CustomerEmail} -> {order.Total}\n");
            _emailService.Send(order.CustomerEmail, order.Status);
        }
    }

    public interface IShippingDelivery
    {
        public bool CanShip(Order order);
    }

    public class RegularShippingDelivery : IShippingDelivery
    {
        public bool CanShip(Order order) => !string.IsNullOrWhiteSpace(order.Address) && order.WeightKg >= 0;

    }

    public class FragileShippingDelivery : IShippingDelivery
    {
        public bool CanShip(Order order) => !string.IsNullOrWhiteSpace(order.Address) && order.WeightKg >= 0 && order.WeightKg < 10;

    }

    public class ShipService
    {
        private IShippingDelivery _shippingDelivery;

        public ShipService(IShippingDelivery shippingDelivery)
        {
            _shippingDelivery = shippingDelivery;
        }

        public void Ship(Order order)
        {
            if (_shippingDelivery.CanShip(order))
                Console.WriteLine("Order is shipped!");
            else
                Console.WriteLine("Cannot ship the order");
        }
    }
}
