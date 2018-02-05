namespace Geetest.Core
{
    public class GeetestRegisterResult
    {
        public bool Success { get; set; }

        public string Challenge { get; set; }

        public string Gt { get; set; }

        public bool NewCaptcha { get; set; }
    }
}