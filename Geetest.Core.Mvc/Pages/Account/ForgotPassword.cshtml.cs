using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Geetest.Core.Mvc.Data;
using Geetest.Core.Mvc.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Geetest.Core.Mvc.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendResetPasswordAsync(Input.Email, callbackUrl);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}