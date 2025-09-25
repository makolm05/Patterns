namespace SingleResponsibility.Wrong
{
    public class Phone
    {
        public string Model { get; }
        public int Price { get; }
        public Phone(string model, int price)
        {
            Model = model;
            Price = price;
        }
    }

    public class MobileStore
    {
        List<Phone> phones = new();
        public void Process()
        {
            // ввод данных
            Console.WriteLine("Введите модель:"); 
            string? model = Console.ReadLine(); //<---- can replace with input interface
            Console.WriteLine("Введите цену:");

            // валидация
            bool result = int.TryParse(Console.ReadLine(), out var price); // <---- can replace with validation interface

            if (result == false || price <= 0 || string.IsNullOrEmpty(model))
            {
                throw new Exception("Некорректно введены данные");
            }
            else
            {
                phones.Add(new Phone(model, price)); // <--- can replace with a phone creator interface
                // сохраняем данные в файл
                using (StreamWriter writer = new StreamWriter("store.txt", true)) // <---- can replace with a file writer interface
                {
                    writer.WriteLine(model);
                    writer.WriteLine(price);
                }
                Console.WriteLine("Данные успешно обработаны");
            }
        }
    }
}
