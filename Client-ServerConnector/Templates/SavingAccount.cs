namespace Client_ServerConnector.Templates
{
    public class SavingAccount:Account
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
        public double Percent { get; set; }
        public int Period { get; private set; }

        public SavingAccount(string cardNumber, int balance, double percent, int period)
        {
            CardNumber = cardNumber;
            Balance = balance;
            Percent = percent;
            Period = period;
        }

    }
}