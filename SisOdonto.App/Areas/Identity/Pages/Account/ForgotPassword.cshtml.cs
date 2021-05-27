using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SisOdonto.Domain.Interfaces.Services;
using SisOdonto.Domain.Utils;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SisOdonto.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        #region Fields

        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;

        #endregion Fields

        #region Constructors

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        #endregion Constructors

        #region Properties

        [BindProperty]
        public InputModel Input { get; set; }

        #endregion Properties

        #region Methods

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    return RedirectToPage("./Login");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var options = _userManager.Options.Password;

                var password = Utils.GeneratePassword(options.RequiredLength, options.RequireNonAlphanumeric, options.RequireDigit, options.RequireLowercase, options.RequireUppercase);

                await _userManager.ResetPasswordAsync(user, code, password);

                _emailService.SendEmail(user.Email, "Novas Credenciais de Acesso - SisOdonto", "Credenciais", $"Usuário: {user.Email} <br> Senha: {password}");

                return RedirectToPage("./Login");
            }

            return Page();
        }

        #endregion Methods

        #region Classes

        public class InputModel
        {
            #region Properties

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}