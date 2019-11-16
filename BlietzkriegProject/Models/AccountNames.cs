using System.Collections.Generic;

namespace UI.Models
{
    public class AccountNames
    {

        public static List<string> Accounts
        {
            get
            {
                return new List<string>(){"Saving account", "Checking account", "Credit account"};
            }
        }
    }
}