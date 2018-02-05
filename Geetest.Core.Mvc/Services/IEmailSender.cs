using System.Threading.Tasks;

namespace Geetest.Core.Mvc.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}