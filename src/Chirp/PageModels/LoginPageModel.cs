using Chirp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.PageModels
{
    public class LoginPageModel
    {
        public string SuccessMessage { get; private set; }

        public void SetMessage(string a_email)
        {
            if (!String.IsNullOrWhiteSpace(a_email))
            {
                SuccessMessage = $"A reset password email has been sent to {a_email}.";
            }
        }
    }
}
