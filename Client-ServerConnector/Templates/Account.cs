namespace Client_ServerConnector.Templates
{
    internal interface Account
    {
         string CardNumber { get; }
         int Balance { get;  }
        void put(int amount);
         void withdraw(int amount);
         
    }

}