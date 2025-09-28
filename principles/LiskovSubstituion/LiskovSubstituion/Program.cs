using BadShop;

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