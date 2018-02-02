using System;
using System.Collections.Generic;
using System.Text;

namespace Geetest.Core
{
    public class RegisterResult
    {
        public bool Success { get; set; }

        public string Challenge { get; set; }

        public string Gt { get; set; }

        public bool NewCaptcha { get; set; }
    }
}
