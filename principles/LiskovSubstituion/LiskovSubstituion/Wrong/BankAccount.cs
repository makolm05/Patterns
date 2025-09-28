namespace LiskovSubstituion.Wrong
{
    public class BankAccount
    {
        protected decimal Balance { get; set; }

        public BankAccount(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive.");

            if (amount > Balance)
                throw new InvalidOperationException("Not enough funds.");

            Balance -= amount;
        }

        public decimal GetBalance() => Balance;
    }

    public class PremiumAccount : BankAccount
    {
        public PremiumAccount(decimal initialBalance) : base(initialBalance) { }

        // ❌ Нарушение предусловий и инвариантов
        public override void Withdraw(decimal amount)
        {
            if (amount > Balance * 2)
                throw new InvalidOperationException("Cannot withdraw more than double balance.");

            Balance -= amount; // ❌ Позволяем уйти в минус, ломаем инвариант
        }
    }
}
