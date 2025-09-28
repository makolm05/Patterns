namespace LiskovSubstituion.Correct
{
    public abstract class Bird
    {
        public virtual void Eat()
        {
            Console.WriteLine("The bird is eating!");
        }
    }

    public class FlyingBird : Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("The bird is flying!");
        }
    }

    public class Eagle : FlyingBird
    {
        public override void Eat()
        {
            Console.WriteLine("The eagle is eating!");
        }

        public override void Fly()
        {
            Console.WriteLine("The eagle is flying!");
        }
    }

    public class Penguin : Bird
    {
        public override void Eat()
        {
            Console.WriteLine("The penguin is eating!");
        }
    }

}
