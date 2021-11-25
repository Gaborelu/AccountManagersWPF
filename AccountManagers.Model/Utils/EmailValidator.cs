using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagers.Common.Utils
{
    public class EmailValidator : IEmailValidator
    {
        public bool IsEmailValid(string email)
        {
            if (email.Contains("@") && email.Contains("."))
            {
                return true;
            }
            return false;
        }
    }
}
