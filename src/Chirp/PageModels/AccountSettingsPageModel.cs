using Chirp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.PageModels
{
    public class AccountSettingsPageModel
    {
        public string SuccessMessage { get; private set; }

        public ChirpUserViewModel UserVM { get; set; }

        public void SetMessage(string a_actionTaken)
        {
            if (a_actionTaken == "PasswordChanged")
            {
                SuccessMessage = "You successfully changed your password.";
            }
        }
    }
}
