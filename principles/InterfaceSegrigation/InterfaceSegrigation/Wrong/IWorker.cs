namespace InterfaceSegrigation.Wrong
{
    public interface IWorker
    {
        void Work();
        void Eat();
        void Sleep();
    }

    public class Robot : IWorker
    {
        public void Work()
        {
            Console.WriteLine("Робот работает без устали.");
        }

        public void Eat()
        {
            // ⚠️ Нарушение — робот не умеет есть
            throw new NotImplementedException();
        }

        public void Sleep()
        {
            // ⚠️ Нарушение — робот не спит
            throw new NotImplementedException();
        }
    }
}
