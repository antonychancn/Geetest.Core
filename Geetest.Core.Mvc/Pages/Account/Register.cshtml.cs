using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Geetest.Core.Mvc.Data;
using Geetest.Core.Mvc.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Geetest.Core.Mvc.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly IGeetestManager _geetestManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IEmailSender emailSender,
            IGeetestManager geetestManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _geetestManager = geetestManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var geetestValidate = await _geetestManager.ValidateAsync(new GeetestValidate
                {
                    Offline = Input.Offline,
                    Challenge = Input.Challenge,
                    Validate = Input.Validate,
                    Seccode = Input.Seccode
                });

                if (!geetestValidate)
                {
                    ModelState.AddModelError(string.Empty, "ÑéÖ¤Âë´íÎó");
                    return Page();
                }

                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(Input.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, false);
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public bool Offline { get; set; }

            [Required]
            public string Challenge { get; set; }

            [Required]
            public string Validate { get; set; }

            [Required]
            public string Seccode { get; set; }
        }
    }
}