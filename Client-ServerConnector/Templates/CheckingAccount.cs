namespace Client_ServerConnector.Templates
{
    public class CheckingAccount :Account
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

        public CheckingAccount(string cardNumber, int balance)
        {
            CardNumber = cardNumber;
            Balance = balance;
        }
    }
}