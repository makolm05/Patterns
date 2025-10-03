namespace InterfaceSegrigation.Correct
{

    public interface IWorkable
    {
        void Work();
    }

    public interface IFeedable
    {
        void Eat();
    }

    public interface ISleepable
    {
        void Sleep();
    }

    public class Robot : IWorkable
    {
        public void Work()
        {
            Console.WriteLine("Робот работает без устали.");
        }
    }

    public class Worker : IWorkable, IFeedable, ISleepable
    {
        public void Work()
        {
            Console.WriteLine("Человек работает.");
        }

        public void Eat()
        {
            Console.WriteLine("Человек ест.");
        }

        public void Sleep()
        {
            Console.WriteLine("Человек спит.");
        }
    }
}
