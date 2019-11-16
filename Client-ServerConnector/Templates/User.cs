using System;

namespace Client_ServerConnector.Templates
{
    internal class User
    {
        public Guid Id { get; private set; }

        public SavingAccount SavingAccount { get; private set; }
        public CheckingAccount CheckingAccount { get; private set; }
        public CreditAccount CreditAccount { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public User(Guid id, SavingAccount savingAccount, CheckingAccount checkingAccount, CreditAccount creditAccount, string email, string password)
        {
            Id = id;
            SavingAccount = savingAccount;
            CheckingAccount = checkingAccount;
            CreditAccount = creditAccount;
            Email = email;
            Password = password;
        }
    }

}