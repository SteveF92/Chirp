﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendGridMessenger

{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridPassword { get; set; }
        public string SendGridKey { get; set; }
    }
}
