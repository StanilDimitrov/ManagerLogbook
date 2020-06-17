using ManagerLogbook.Data.Models;
using ManagerLogbook.Web.Controllers;
using ManagerLogbook.Web.Models.AccountViewModels;
using ManagerLogbook.Web.Services;
using ManagerLogbook.Web.Services.Contracts;
using ManagerLogbook.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CreateController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IImageOptimizer optimizer;

        public CreateController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            ILogger<CreateController> logger,
            IImageOptimizer optimizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            this.optimizer = optimizer;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                string imageName = null;

                if (model.UserImage != null)
                {
                    imageName = optimizer.OptimizeImage(model.UserImage, 400, 800);
                }

                if (model.Image != null)
                {
                    optimizer.DeleteOldImage(model.Image);
                }
                var user = new User {  Email = model.Email, UserName = model.UserName, Picture = imageName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (model.UserRole == "Manager")
                    {
                        await _userManager.AddToRoleAsync(user, "Manager");
                    }
                    else if (model.UserRole == "Moderator")
                    {
                        await _userManager.AddToRoleAsync(user, "Moderator");
                    }
                    else if (model.UserRole == "Admin")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    _logger.LogInformation("User created a new account with password.");
                    return Ok(string.Format(WebConstants.UserCreated, model.UserName));
                }
                return BadRequest("Invalid attempt.");
            }

            return BadRequest(WebConstants.EnterValidData);
        }

           #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}