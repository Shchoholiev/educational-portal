using EducationalPortal.Application.Interfaces;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.Paging;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EducationalPortal.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PageParameters pageParameters)
        {
            IEnumerable<CartItem> cartItems = new List<CartItem>();
            var totalCount = new Int32();

            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                cartItems = (await this._shoppingCartService.GetPageAsync(userEmail,
                             pageParameters.PageSize, pageParameters.PageNumber));
                totalCount = await this._shoppingCartService.GetCountAsync(userEmail);
            }
            else
            {
                var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
                if (cookies != null)
                {
                    var deserialised = (await this._shoppingCartService.GetDeserialisedAsync(cookies));
                    cartItems = deserialised.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                            .Take(pageParameters.PageSize);
                    totalCount = deserialised.Count();
                }
            }

            var pagedList = new PagedList<CartItem>(cartItems, pageParameters, totalCount);
            return View(pagedList);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int courseId)
        {
            var userEmail = String.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (!await this._shoppingCartService.Exists(courseId, userEmail))
                {
                    var cartItem = new CartItem
                    {
                        Course = new Course { Id = courseId },
                        User = new User { Email = userEmail }
                    };

                    await this._shoppingCartService.AddAsync(cartItem);
                }
            }
            else
            {
                var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
                if (cookies == null || !cookies.Contains($"{courseId}"))
                {
                    cookies += $"-{courseId}";
                    this.SaveCookies(cookies);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            var userEmail = String.Empty;
            if (User.Identity.IsAuthenticated)
            {
                userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                await this._shoppingCartService.DeleteAsync(cartItemId);
            }
            else
            {
                var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
                var matchRegex = new Regex(string.Format($"(-)?{cartItemId}"));
                var newCookies = matchRegex.Replace(cookies, string.Empty);
                this.SaveCookies(newCookies);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Buy()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                await this._shoppingCartService.BuyAsync(userEmail);
                return RedirectToAction("MyLearning", "Account");
            }
            else
            {
                return RedirectToAction("Register", "Account", new { returnUrl = "/ShoppingCart/Buy" });
            }
        }

        private void SaveCookies(string cookies)
        {
            var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("EducationalPortal_ShoppingCart", cookies, cookieOptions);
        }
    }
}
