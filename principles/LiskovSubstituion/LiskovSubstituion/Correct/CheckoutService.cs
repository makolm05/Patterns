namespace LiskovSubstituion.Correct
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

    public enum DiscountType
    {
        None,
        Percent10,
        BlackFriday,
        Student
    };

    public interface IDiscountProvider
    {
        IDiscount? Get(DiscountType type);
    }

    public class DiscountProvider : IDiscountProvider
    {
        private readonly Dictionary<DiscountType, IDiscount> _map;

        public DiscountProvider(IEnumerable<(DiscountType, IDiscount)> discounts)
        {
            _map = discounts.ToDictionary(d => d.Item1, d => d.Item2);
        }

        public IDiscount? Get(DiscountType type) =>
            _map.TryGetValue(type, out var strategy) ? strategy : null;
    }

    public interface IDiscount
    {
        decimal Apply(decimal amount);
    }

    public class BaseDiscount : IDiscount
    {
        protected decimal _discountPercent { get; set; } = 0m;

        public virtual decimal Apply(decimal amount)
        {
            var after = amount - amount * _discountPercent;
            return after < 0 ? 0 : after;
        }
    }

    public class Percent10Discount : BaseDiscount
    {
        public Percent10Discount() => _discountPercent = 0.10m;
    }

    public class BlackFridayDiscount : IDiscount
    {
        public decimal Apply(decimal amount)
        {
            var after = amount >= 100 ? amount - 30 : amount - 10;
            return after < 0 ? 0 : after;
        }
    }

    public class StudentDiscount : BaseDiscount
    {
        public StudentDiscount() => _discountPercent = 0.15m;
    }

    public interface ITaxService
    {
        decimal Compute(decimal amountAfterDiscount);
    }

    public class TaxServiceBase : ITaxService
    {
        protected decimal _taxRate { get; set; } = 0.07m; // default: US 7%

        public virtual decimal Compute(decimal amountAfterDiscount)
            => amountAfterDiscount * _taxRate;
    }

    public class USTaxService : TaxServiceBase
    {
        public USTaxService() { _taxRate = 0.07m; }
    }

    public class DETaxService : TaxServiceBase
    {
        public DETaxService() { _taxRate = 0.19m; }
    }

    public class UATaxService : TaxServiceBase
    {
        public UATaxService() { _taxRate = 0.20m; }
    }

    public interface IEmailService
    {
        public void SendEmail(string to, string body);
    }

    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string body)
        {
            Console.WriteLine($"Email to {to}: {body}");
        }
    }

    public class CheckoutService
    {
        private readonly IDiscountProvider _discountProvider;
        private readonly IDiscount _discountService;
        private readonly ITaxService _taxService;
        private readonly IEmailService _emailService;
        public CheckoutService(IDiscountProvider discountProvider, IDiscount discountService, ITaxService taxService, IEmailService emailService)
        {
            _discountProvider = discountProvider;
            _discountService = discountService;
            _taxService = taxService;
            _emailService = emailService;
        }

        public decimal Checkout(Order2 order, DiscountType type)
        {
            var discount = _discountProvider.Get(type);
            if (discount is null)
                throw new ArgumentException($"Discount {type} is not supported.");

            var costAfterDiscount = _discountService.Apply(order.Subtotal);
            var total = _taxService.Compute(costAfterDiscount);
            _emailService.SendEmail(order.CustomerEmail, $"Your total is {total:C}");

            return total;
        }
    }
}
