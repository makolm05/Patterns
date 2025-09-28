namespace BadShop
{
    // Модель домена
    public class Order
    {
        public decimal Subtotal { get; set; }
        public string CustomerEmail { get; set; } = "";
        public string Country { get; set; } = "UA";
        public List<string> Items { get; } = new();
        public decimal Total { get; set; }           // инвариант (должен быть >= 0) — НАРУШИМ
        public string Status { get; set; } = "New";  // инвариант (ограниченный набор) — НАРУШИМ
        public decimal WeightKg { get; set; }
        public string? Address { get; set; }
    }

    // ❌ SRP НАРУШЕНО: «Бог-объект» делает всё — валидирует, считает, логирует, пишет на диск, шлёт письма
    // ❌ OCP НАРУШЕНО: для нового типа скидки придётся менять switch
    // ❌ LSP (инвариант/постусловие) НАРУШЕНО: ставит Total < 0 и произвольный Status
    public class OrderProcessor
    {
        public void Process(Order order, string discountType, bool express)
        {
            // «валидация»
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (order.Subtotal <= 0) throw new ArgumentException("Bad subtotal");

            // "бизнес-логика" с жёстким switch (OCP)
            var calculator = express ? new ExpressPriceCalculator() : new PriceCalculator();
            order.Total = calculator.CalculateTotal(order, discountType);

            // ❌ постусловие/инвариант: допускаем отрицательный total
            if (order.Total < 0)
                Console.WriteLine("Ну и ладно, бывает…"); // тишина вместо защиты инварианта

            // ❌ инвариант: статусы произвольные, вплоть до мусорных
            order.Status = order.Total > 0 ? "Paid-ish" : "¯\\_(ツ)_/¯";

            // Побочные эффекты (SRP): логирование, запись в файл, «отправка email»
            File.AppendAllText("orders.log", $"{DateTime.Now}: {order.CustomerEmail} -> {order.Total}\n");
            Console.WriteLine("Email sent to " + order.CustomerEmail);
        }
    }

    // Базовый калькулятор
    public class PriceCalculator
    {
        // Контракт (неформальный):
        //   Предусловие: order.Subtotal > 0
        //   Постусловие: результат >= 0 и >= некоторых минимальных сборов
        public virtual decimal CalculateTotal(Order order, string discountType)
        {
            decimal total = order.Subtotal;

            // ❌ OCP: добавление скидки => менять код
            switch (discountType)
            {
                case "None":
                    break;
                case "Student":
                    total *= 0.9m;
                    break;
                case "VIP":
                    total *= 0.8m;
                    break;
                default:
                    // ❌ «не поддерживается» — тоже запах: на каждое расширение придётся сюда лезть
                    throw new NotSupportedException($"Unknown discount '{discountType}'");
            }

            // «минимальная логистика»
            total += 50m; // базовая доставка
            return total; // по контракту — неотрицательно
        }
    }

    // ❌ LSP — ПРЕДУСЛОВИЕ УСИЛЕНО: базовый принимал любой subtotal > 0,
    // а наследник требует минимум 50 (усилил требования к клиенту)
    // ❌ LSP — ПОСТУСЛОВИЕ ОСЛАБЛЕНО: может вернуть отрицательный total
    // ❌ LSP — ИНВАРИАНТ БАЗЫ ЛОМАЕМ: игнорируем идею «total >= 0»
    public class ExpressPriceCalculator : PriceCalculator
    {
        public override decimal CalculateTotal(Order order, string discountType)
        {
            if (order.Subtotal < 50)
                throw new ArgumentException("Express доступен только для заказов от 50"); // усиление предусловия

            var total = order.Subtotal;

            // «секретная» скидка — 120% (перебор ради демонстрации)
            if (discountType == "SuperVIP")
                total *= -1.2m; // ❌ постусловие/инвариант — отрицательный total

            // «экспресс сбор»
            total += 100m;

            return total;
        }
    }

    // Ещё одна иерархия для демонстрации предусловия
    public class ShippingService
    {
        // Контракт:
        //   Предусловие: Address != null, Weight > 0
        public virtual void Ship(Order order)
        {
            if (order.Address is null) throw new ArgumentException("Address required");
            if (order.WeightKg <= 0) throw new ArgumentException("Weight must be > 0");
            Console.WriteLine("Shipped by standard");
        }
    }

    public class FragileShippingService : ShippingService
    {
        // ❌ LSP — ПРЕДУСЛОВИЕ УСИЛЕНО: базовому было всё равно на вес,
        // а тут запрещаем > 10 кг
        public override void Ship(Order order)
        {
            if (order.WeightKg > 10)
                throw new InvalidOperationException("Fragile shipping up to 10kg only"); // усилили
            base.Ship(order);
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var order = new Order
            {
                Subtotal = 40,                 // провоцируем усиление предусловия в ExpressPriceCalculator
                CustomerEmail = "user@example.com",
                Country = "UA",
                WeightKg = 12,                 // провоцируем усиление предусловия в FragileShippingService
                Address = "Kyiv, UA"
            };

            var processor = new OrderProcessor();

            try
            {
                // ❌ В этом вызове могут проявиться сразу все поломки
                processor.Process(order, discountType: "SuperVIP", express: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Boom: " + ex.Message);
            }

            try
            {
                new FragileShippingService().Ship(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Boom shipping: " + ex.Message);
            }
        }
    }
}