using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCart([FromQuery]PageParameters pageParameters)
        {
            var cartItems = await this._shoppingCartService.GetPageAsync(pageParameters, Email);
            this.SetPagingMetadata(cartItems);
            return cartItems;
        }

        [AllowAnonymous]
        [HttpGet("page-from-cookie")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCart([FromQuery] string cookie,
                                                                       [FromQuery] PageParameters pageParameters)
        {
            var cartItems = await this._shoppingCartService.GetPageFromCookieAsync(pageParameters, cookie);
            this.SetPagingMetadata(cartItems);
            return cartItems;
        }

        [HttpGet("total-price")]
        public async Task<ActionResult<int>> GetTotalPrice()
        {
            return await this._shoppingCartService.GetTotalPriceAsync(Email);
        }

        [AllowAnonymous]
        [HttpGet("total-price-from-cookie")]
        public async Task<ActionResult<int>> GetTotalPrice([FromQuery] string cookie)
        {
            return await this._shoppingCartService.GetTotalPriceFromCookieAsync(cookie);
        }


        [HttpPut("add-to-cart/{courseId}")]
        public async Task<IActionResult> AddToCart(int courseId)
        {
            await this._shoppingCartService.AddToCartAsync(courseId, Email);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._shoppingCartService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("buy")]
        public async Task<IActionResult> Buy()
        {
            await this._shoppingCartService.BuyAsync(Email);
            return Ok();
        }
    }
}
