using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Infrastructure.Identity;
using EducationalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        private readonly IUserManager _userManager;

        public AccountController(IUserService userService, IUserManager userManager)
        {
            this._userService = userService;
            this._userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            returnUrl = this.CheckReturnUrl(returnUrl);
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Name = model.Name, Email = model.Email, Password = model.Password };
                var result = await this._userService.RegisterAsync(userDTO);
                
                if (result.Succeeded)
                {
                    await this._userManager.SignInAsync(this.HttpContext, userDTO, false);
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    foreach (var error in result.Messages)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            returnUrl = this.CheckReturnUrl(returnUrl);
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Email = model.Email, Password = model.Password };
                var result = await this._userService.LoginAsync(userDTO);

                if (result.Succeeded)
                {
                    var user = await this._userService.GetUserAsync(userDTO.Email);
                    userDTO.Name = user.Name;
                    await this._userManager.SignInAsync(this.HttpContext, userDTO, model.RememberMe);
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    foreach (var error in result.Messages)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this._userManager.SignOutAsync(this.HttpContext);
            return RedirectToAction("Index", "Home");
        }

        private string CheckReturnUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)
               || returnUrl.Contains("Register")
               || returnUrl.Contains("Login"))
            {
                return "/";
            }
            return returnUrl;
        }
    }
}
