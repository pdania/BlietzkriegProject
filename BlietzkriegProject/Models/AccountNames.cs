using System.Collections.Generic;

namespace BlietzkriegProject.Models
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