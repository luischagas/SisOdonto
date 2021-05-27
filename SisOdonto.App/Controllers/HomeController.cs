using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SisOdonto.App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SisOdonto.App.Areas.Identity.Pages.Account.Manage;
using SisOdonto.Application.Models.Common;

namespace SisOdonto.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View("_ChangePassword", new UpdatePasswordModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeNewPassword(UpdatePasswordModel request)
        {
            if (ModelState.IsValid is false)
                return PartialView("_ChangePassword");

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (changePasswordResult.Succeeded is false)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return PartialView("_ChangePassword");
            }

            await _signInManager.RefreshSignInAsync(user);

            var url = Url.Action("Index", "Dentist");

            return Json(new { success = true, url });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
