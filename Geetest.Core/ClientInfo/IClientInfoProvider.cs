namespace Geetest.Core
{
    public interface IClientInfoProvider
    {
        string UserId { get; }

        string Sdk { get; }

        string ClientType { get; }

        string IpAddress { get; }
    }
}