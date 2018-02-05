using System.Threading.Tasks;

namespace Geetest.Core
{
    public interface IGeetestManager
    {
        Task<GeetestRegisterResult> RegisterAsync();

        Task<bool> ValidateAsync(GeetestValidate geetestValidate);
    }
}