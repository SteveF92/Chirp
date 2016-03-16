using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.PageModels
{
    public class ResetPasswordPageModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
