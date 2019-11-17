namespace UI.Templates
{
    public class StationManager
    {
        public static User CurrentUser { get; set; }

        public static Account GetAccount(string selectedAccount)
        {

            if (selectedAccount.Equals("Checking account"))
            {
                foreach (var account in StationManager.CurrentUser.Accounts)
                    if (account.Percent.Equals(0))
                        return account;
            }
            else if (selectedAccount.Equals("Saving account"))
            {
                foreach (var account in StationManager.CurrentUser.Accounts)
                    if (account.Percent > 0)
                        return account;
            }
            else if (selectedAccount.Equals("Credit account"))
                foreach (var account in StationManager.CurrentUser.Accounts)
                    if (account.Percent < 0)
                        return account;

            return null;
        }

        public static string AccountType(string cardNumber)
        {
            Account account=null;
            foreach (var cardAccount in StationManager.CurrentUser.Accounts)
                if (cardAccount.CardNumber == cardNumber)
                    account = cardAccount;
            if (account.Percent.Equals(0))
            {
                return "Checking account";
            }
            if (account.Percent<0)
            {
                return "Credit account";
            }

            return "Saving account";
        }

        public static ScheduleTranferDto CurrentScheduledTransfer { get; set; }
    }
}