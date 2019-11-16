namespace Client_ServerConnector.Templates
{
    public class CreditAccount: Account
    {
        public string CardNumber { get; private set; }
        public int Balance { get; private set; }
        public void put(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void withdraw(int amount)
        {
            throw new System.NotImplementedException();
        }
        public int Limit { get;
            set;
        }
        public double Percent { get; private set; }
        public int expiryDate { get; private set; }

        public CreditAccount(string cardNumber, int balance, int limit, double percent, int expiryDate)
        {
            CardNumber = cardNumber;
            Balance = balance;
            Limit = limit;
            Percent = percent;
            this.expiryDate = expiryDate;
        }

    }
}