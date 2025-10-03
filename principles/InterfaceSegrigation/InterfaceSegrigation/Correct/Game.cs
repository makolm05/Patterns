namespace InterfaceSegrigation.Correct
{
    public interface IWalkable
    {
        void MoveTo(int x, int y);
    }

    public interface IAttacker
    {
        void Attack(object target);
    }

    public interface ISpellCaster
    {
        void CastSpell(string spell, object target);
    }

    public interface IFlyable
    {
        void FlyTo(int x, int y);
    }

    public interface ISwimmable
    {
        void SwimTo(int x, int y);
    }

    public interface IFighter : IFlyable, IAttacker
    {

    }

    public class Soldier : IWalkable, IAttacker, ISwimmable
    {
        public void MoveTo(int x, int y)
        {
            Console.WriteLine($"Soldier moves to {x},{y}");
        }
        public void Attack(object target)
        {
            Console.WriteLine("Soldier attacks");
        }
        public void SwimTo(int x, int y)
        {
            Console.WriteLine($"Soldier swims to {x},{y}");
        }
    }

    public class Dragon : IWalkable, ISpellCaster, IFighter
    {
        public void MoveTo(int x, int y)
        {
            Console.WriteLine($"Dragon moves to {x},{y}");
        }

        public void Attack(object target)
        {
            Console.WriteLine("Dragon bites");
        }

        public void CastSpell(string spell, object target)
        {
            Console.WriteLine($"Dragon casts {spell} on {target}");
        }

        public void FlyTo(int x, int y)
        {
            Console.WriteLine($"Dragon flies to {x},{y}");
        }
    }

    public class AirFighter : IFighter
    {
        public void Attack(object target)
        {
            Console.WriteLine("Fighter attacks");
        }

        public void FlyTo(int x, int y)
        {
            Console.WriteLine($"Fighter flies to {x},{y}");
        }
    }

    public class Fish : ISwimmable
    {
        public void SwimTo( int x, int y)
        {
            Console.WriteLine($"Fish swims to {x},{y}");
        }
    }

    public class PatrolSystem
    {
        public void SendPatrol(IWalkable unit)
        {
            unit.MoveTo(10, 20);
        }
    }

    public class AirRaidController
    {
        public void OrderAirStrike(IFighter unit, int x, int y)
        {
            unit.FlyTo(x, y);
            unit.Attack(new object());
        }
    }

    public class WaterMission
    {
        public void Escort(ISwimmable unit, int x, int y)
        {
            unit.SwimTo(x, y);
        }
    }
}
