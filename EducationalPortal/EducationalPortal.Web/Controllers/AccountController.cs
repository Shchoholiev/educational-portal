﻿using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.Identity;
using EducationalPortal.Web.Paging;
using EducationalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPortal.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        private readonly IShoppingCartService _shoppingCartService;

        private readonly IUserManager _userManager;

        private readonly ICloudStorageService _cloudStorageService;

        public AccountController(IUserService userService, IUserManager userManager,
                                 ICloudStorageService cloudStorageService,
                                 IShoppingCartService shoppingCartService)
        {
            this._userService = userService;
            this._userManager = userManager;
            this._cloudStorageService = cloudStorageService;
            this._shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._userService.GetUserWithSkillsAsync(email);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserDTO userDTO, IFormFile avatar)
        {
            var user = await this._userService.GetUserAsync(userDTO.Email);
            user.Name = userDTO.Name;
            user.Email = userDTO.Email;

            if (avatar != null)
            {
                using (var stream = avatar.OpenReadStream())
                {
                    var uri = await this._cloudStorageService.UploadAsync(stream, avatar.FileName, 
                                                                          avatar.ContentType, "avatars");
                    await this._cloudStorageService.DeleteAsync(user.Avatar, "avatars");
                    user.Avatar = uri;
                }
            }
            await this._userService.UpdateUserAsync(user);

            return RedirectToAction("Profile", new { email = user.Email });
        }

        [HttpGet]
        public async Task<IActionResult> MyLearning(PageParameters pageParameters)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usersCourses = await this._userService.GetUsersCoursesPageAsync(email, 
                                    pageParameters.PageSize, pageParameters.PageNumber);
            var totalCount = await this._userService.GetUsersCoursesCountAsync(email);
            var pagedList = new PagedList<UsersCourses>(usersCourses, pageParameters, totalCount);

            return View(pagedList);
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
                    await this.CheckShoppingCartCookies(userDTO.Email);
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

        private async Task CheckShoppingCartCookies(string userEmail)
        {
            var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
            if (cookies != null)
            {
                var cartItems = await this._shoppingCartService.GetDeserialisedAsync(cookies);
                foreach (var cartItem in cartItems)
                {
                    cartItem.User = new User { Email = userEmail };
                    await this._shoppingCartService.AddAsync(cartItem);
                    var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(-1) };
                    Response.Cookies.Append("EducationalPortal_ShoppingCart", "", cookieOptions);
                }
            }
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
