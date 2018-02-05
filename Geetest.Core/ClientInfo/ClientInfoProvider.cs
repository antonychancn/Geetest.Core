namespace Geetest.Core
{
    public class ClientInfoProvider : IClientInfoProvider
    {
        public string UserId => "Geetest.Core";

        public string Sdk => "Geetest.Core";

        public string ClientType => "unknown";

        public string IpAddress => "unknown";
    }
}