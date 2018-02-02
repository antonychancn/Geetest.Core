using System;
using System.Collections.Generic;
using System.Text;

namespace Geetest.Core.Configuration
{
    public class GeetestConfiguration : IGeetestConfiguration
    {
        public GeetestConfiguration()
        {
            Protocol = "https://";
            ApiServerUrl = "api.geetest.com";
            ApiRegisterUrl = "/register.php";
            ApiValidateUrl = "/validate.php";
            NewCaptcha = true;
            JsonFormat = true;
        }

        public string Id { get; set; }

        public string Key { get; set; }

        public string Protocol { get; set; }

        public string ApiServerUrl { get; set; }

        public string ApiValidateUrl { get; set; }

        public string ApiRegisterUrl { get; set; }

        public bool NewCaptcha { get; set; }

        public bool JsonFormat { get; set; }
        public string ToQueryString()
        {
            var ip = "";
            var jsonFormat = JsonFormat ? "1" : "0";
            return $"gt={Id}&json_format={jsonFormat}&sdk=Geetest.Core&client_type=Geetest.Core&ip_address={ip}";
        }
    }
}
