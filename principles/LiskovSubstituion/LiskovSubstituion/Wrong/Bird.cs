namespace LiskovSubstituion.Wrong
{
    public class Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("The bird is flying!");
        }
    }

    public class Eagle : Bird
    {
        public override void Fly()
        {
            Console.WriteLine("The eagle is flying!");
        }
    }

    public class Penguin : Bird
    {
        public override void Fly()
        {
            throw new NotSupportedException("Penguins cannot fly!");
        }
    }
}
