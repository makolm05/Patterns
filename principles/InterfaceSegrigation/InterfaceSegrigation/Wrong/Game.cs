namespace InterfaceSegrigation.Wrong
{
    public interface IUnit
    {
        void Move(int x, int y);
        void Attack(object target);
        void CastSpell(string spell, object target);
        void FlyTo(int x, int y);
        void SwimTo(int x, int y);
        void BurrowTo(int x, int y);
    }

    public class Soldier : IUnit
    {
        public void Move(int x, int y) => Console.WriteLine($"Soldier moves to {x},{y}");
        public void Attack(object target) => Console.WriteLine("Soldier attacks");

        public void CastSpell(string spell, object target)
            => throw new NotImplementedException(); // ❌

        public void FlyTo(int x, int y)
            => throw new NotImplementedException(); // ❌

        public void SwimTo(int x, int y)
            => throw new NotImplementedException(); // ❌

        public void BurrowTo(int x, int y)
            => throw new NotImplementedException(); // ❌
    }

    public class Dragon : IUnit
    {
        public void Move(int x, int y) => Console.WriteLine($"Dragon walks to {x},{y}");
        public void Attack(object target) => Console.WriteLine("Dragon bites");

        public void CastSpell(string spell, object target)
            => Console.WriteLine($"Dragon casts {spell}");

        public void FlyTo(int x, int y)
            => Console.WriteLine($"Dragon flies to {x},{y}");

        public void SwimTo(int x, int y)
            => throw new NotImplementedException(); // ❌

        public void BurrowTo(int x, int y)
            => throw new NotImplementedException(); // ❌
    }

    public class Fish : IUnit
    {
        public void Move(int x, int y) => Console.WriteLine($"Fish swims (ground move) to {x},{y}?"); // 🤔
        public void Attack(object target) => Console.WriteLine("Fish bumps (weak)");

        public void CastSpell(string spell, object target)
            => throw new NotImplementedException(); // ❌

        public void FlyTo(int x, int y)
            => throw new NotImplementedException(); // ❌

        public void SwimTo(int x, int y)
            => Console.WriteLine($"Fish swims to {x},{y}");

        public void BurrowTo(int x, int y)
            => throw new NotImplementedException(); // ❌
    }

    public class PatrolSystem
    {
        public void SendPatrol(IUnit unit)
        {
            unit.Move(10, 20); // Нужен только Move
        }
    }

    public class AirRaidController
    {
        public void OrderAirStrike(IUnit unit, int x, int y)
        {
            unit.FlyTo(x, y);  // Требуется умение летать
            unit.Attack(new object());
        }
    }

    public class WaterMission
    {
        public void Escort(IUnit unit, int x, int y)
        {
            unit.SwimTo(x, y); // Требуется плавать
        }
    }
}
