namespace Geetest.Core.Configuration
{
    public interface IGeetestConfiguration
    {
        string Id { get; set; }

        string Key { get; set; }

        string Protocol { get; set; }

        string ApiServerUrl { get; set; }

        string ApiValidateUrl { get; set; }

        string ApiRegisterUrl { get; set; }

        bool NewCaptcha { get; set; }

        bool JsonFormat { get; set; }

        string ToQueryString();
    }
}