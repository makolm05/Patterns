namespace LiskovSubstituion.Correct
{
    public abstract class BankAccount
    {
        protected decimal Balance { get; set; }

        protected BankAccount(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public abstract void Withdraw(decimal amount);

        public decimal GetBalance() => Balance;
    }

    public class StandardAccount : BankAccount
    {
        public StandardAccount(decimal initialBalance) : base(initialBalance) { }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive.");

            if (amount > Balance)
                throw new InvalidOperationException("Not enough funds."); // Предусловие + инвариант

            Balance -= amount;
        }
    }

    public class PremiumAccount : BankAccount
    {
        private readonly decimal _creditLimit;

        public PremiumAccount(decimal initialBalance, decimal creditLimit) : base(initialBalance)
        {
            _creditLimit = creditLimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive."); // предусловие

            if (Balance - amount < -_creditLimit)
                throw new InvalidOperationException("Exceeded credit limit."); // постусловие + инвариант

            Balance -= amount;
        }
    }
}
