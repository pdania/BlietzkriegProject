using System.Collections.Generic;

namespace UI.Templates
{
    public class User
    {
            public string Email { get; set; }
            public string Login { get; set; }
            public string Token { get; set; }

            public ICollection<Account> Accounts;
    }

}