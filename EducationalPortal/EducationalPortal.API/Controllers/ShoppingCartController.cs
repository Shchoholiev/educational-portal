using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCart([FromQuery]PageParameters pageParameters)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var cartItems = await this._shoppingCartService.GetPageAsync(email, pageParameters);
            
            var metadata = new
            {
                cartItems.TotalItems,
                cartItems.PageSize,
                cartItems.PageNumber,
                cartItems.TotalPages,
                cartItems.HasNextPage,
                cartItems.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return cartItems;
        }

        [HttpGet("total-price")]
        public async Task<ActionResult<int>> GetTotalPrice()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            return await this._shoppingCartService.GetTotalPrice(email);
        }


        [HttpPut("add-to-cart/{courseId}")]
        public async Task<IActionResult> AddToCart(int courseId)
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

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            await this._shoppingCartService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("buy")]
        public async Task<IActionResult> Buy()
        {
            var userEmail = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            await this._shoppingCartService.BuyAsync(userEmail);
            return Ok();
        }
    }
}
