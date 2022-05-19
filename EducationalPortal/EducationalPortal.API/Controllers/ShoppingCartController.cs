using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EducationalPortal.API.Controllers
{
    [ApiController]
    [Route("api/shopping-cart")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCart([FromQuery]PageParameters pageParameters)
        {
            var pagedCart = new PagedList<Course>();

            if (User.Identity.IsAuthenticated)
            {
                var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var cartItems = await this._shoppingCartService.GetPageAsync(email, pageParameters);
                pagedCart = new PagedList<Course>(cartItems.Select(ci => ci.Course), pageParameters, cartItems.TotalItems);
            }
            else
            {
                var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
                if (cookies != null)
                {
                    var deserialised = (await this._shoppingCartService.GetDeserialisedAsync(cookies));
                    var cartItems = deserialised.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                                .Take(pageParameters.PageSize);
                    var totalItems = deserialised.Count();
                    pagedCart = new PagedList<Course>(cartItems.Select(ci => ci.Course), pageParameters, totalItems);
                }
            }

            var metadata = new
            {
                pagedCart.TotalItems,
                pagedCart.PageSize,
                pagedCart.PageNumber,
                pagedCart.TotalPages,
                pagedCart.HasNextPage,
                pagedCart.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return pagedCart;
        }

        [HttpGet("total-price")]
        public async Task<ActionResult<int>> GetTotalPrice()
        {
            if (User.Identity.IsAuthenticated)
            {
                var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                return await this._shoppingCartService.GetTotalPrice(email);
            }
            else
            {
                var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
                if (cookies != null)
                {
                    var deserialised = (await this._shoppingCartService.GetDeserialisedAsync(cookies));
                    return deserialised.Sum(ci => ci.Course.Price);
                }
            }

            return 0;
        }


        [HttpPut("add-to-cart/{courseId}")]
        public async Task<IActionResult> AddToCart(int courseId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
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

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                await this._shoppingCartService.DeleteAsync(id);
            }
            else
            {
                var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
                var matchRegex = new Regex(string.Format($"(-)?{id}"));
                var newCookies = matchRegex.Replace(cookies, string.Empty);
                this.SaveCookies(newCookies);
            }

            return NoContent();
        }

        [HttpPut("buy")]
        public async Task<IActionResult> Buy()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                await this._shoppingCartService.BuyAsync(userEmail);
                Response.Redirect("/api/account/my-learning");
                return Ok();
            }
            else
            {
                return StatusCode(302, new { uri = "/api/account/register" });
            }
        }

        private void SaveCookies(string cookies)
        {
            var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("EducationalPortal_ShoppingCart", cookies, cookieOptions);
        }
    }
}
