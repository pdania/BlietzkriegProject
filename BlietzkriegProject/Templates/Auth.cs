using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Templates
{
    class Auth
    {
        public string Code { get; set; }

        public Auth(string code)
        {
            Code = code;
        }
    }
}
