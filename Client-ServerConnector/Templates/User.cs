using System;
using System.Collections.Generic;

namespace Client_ServerConnector.Templates
{
    public class User
    {
            public string Email { get; set; }
            public string Login { get; set; }
            public string Token { get; set; }

            public ICollection<User> Accounts;

    }

}