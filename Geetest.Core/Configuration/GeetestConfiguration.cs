namespace Geetest.Core.Configuration
{
    public class GeetestConfiguration : IGeetestConfiguration
    {
        private readonly IClientInfoProvider _clientInfoProvider;

        public GeetestConfiguration(IClientInfoProvider clientInfoProvider)
        {
            _clientInfoProvider = clientInfoProvider;

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
            var jsonFormat = JsonFormat ? "1" : "0";
            return
                $"gt={Id}&json_format={jsonFormat}&user_id={_clientInfoProvider.UserId}&sdk={_clientInfoProvider.Sdk}&client_type={_clientInfoProvider.ClientType}&ip_address={_clientInfoProvider.IpAddress}";
        }
    }
}