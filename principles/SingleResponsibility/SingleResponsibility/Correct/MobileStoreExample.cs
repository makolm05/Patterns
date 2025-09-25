namespace SingleResponsibility.Correct
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

    public interface IMobileStore {
        public void Process();
    }

    public class MobileStore : IMobileStore
    {
        List<Phone> phones = new List<Phone>();

        public IPhoneReader Reader { get; set; }
        public IPhoneBinder Binder { get; set; }
        public IPhoneValidator Validator { get; set; }
        public IPhoneSaver Saver { get; set; }

        public MobileStore(IPhoneReader reader, IPhoneBinder binder, IPhoneValidator validator, IPhoneSaver saver)
        {
            this.Reader = reader;
            this.Binder = binder;
            this.Validator = validator;
            this.Saver = saver;
        }

        public void Process()
        {
            string?[] data = Reader.GetInputData();
            Phone phone = Binder.CreatePhone(data);
            if (Validator.IsValid(phone))
            {
                phones.Add(phone);
                Saver.Save(phone, "store.txt");
                Console.WriteLine("Данные успешно обработаны");
            }
            else
            {
                Console.WriteLine("Некорректные данные");
            }
        }
    }

    public interface IPhoneReader
    {
        string?[] GetInputData();
    }
    public class ConsolePhoneReader : IPhoneReader
    {
        public string?[] GetInputData()
        {
            Console.WriteLine("Введите модель:");
            string? model = Console.ReadLine();
            Console.WriteLine("Введите цену:");
            string? price = Console.ReadLine();
            return new string?[] { model, price };
        }
    }

    public interface IPhoneBinder
    {
        Phone CreatePhone(string?[] data);
    }

    public class GeneralPhoneBinder : IPhoneBinder
    {
        public Phone CreatePhone(string?[] data)
        {
            if (data is { Length: 2 } && data[0] is string model &&
                model.Length > 0 && int.TryParse(data[1], out var price))
            {
                return new Phone(model, price);

            }
            throw new Exception("Ошибка привязчика модели Phone. Некорректные данные");
        }
    }

    public interface IPhoneValidator
    {
        bool IsValid(Phone phone);
    }

    public class GeneralPhoneValidator : IPhoneValidator
    {
        public bool IsValid(Phone phone) =>
            !string.IsNullOrEmpty(phone.Model) && phone.Price > 0;
    }

    public interface IPhoneSaver
    {
        void Save(Phone phone, string fileName);
    }

    public class TextPhoneSaver : IPhoneSaver
    {
        public void Save(Phone phone, string fileName)
        {
            using StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(phone.Model);
            writer.WriteLine(phone.Price);
        }
    }
}
