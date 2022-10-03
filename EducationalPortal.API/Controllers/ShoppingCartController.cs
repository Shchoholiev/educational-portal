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
        public async Task<IEnumerable<CartItemDto>> GetCartAsync([FromQuery] PageParameters pageParameters, 
                                                                 CancellationToken cancellationToken)
        {
            var cartItems = await this._shoppingCartService.GetPageAsync(pageParameters, Email, cancellationToken);
            this.SetPagingMetadata(cartItems);
            return cartItems;
        }

        [AllowAnonymous]
        [HttpGet("page-from-cookie")]
        public async Task<IEnumerable<CartItemDto>> GetCartAsync([FromQuery] string cookie, 
                                                                 [FromQuery] PageParameters pageParameters, 
                                                                 CancellationToken cancellationToken)
        {
            var cartItems = await this._shoppingCartService
                .GetPageFromCookieAsync(pageParameters, cookie, cancellationToken);
            this.SetPagingMetadata(cartItems);
            return cartItems;
        }

        [HttpGet("total-price")]
        public async Task<int> GetTotalPriceAsync(CancellationToken cancellationToken)
        {
            return await this._shoppingCartService.GetTotalPriceAsync(Email, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("total-price-from-cookie")]
        public async Task<int> GetTotalPriceAsync([FromQuery] string cookie, CancellationToken cancellationToken)
        {
            return await this._shoppingCartService.GetTotalPriceFromCookieAsync(cookie, cancellationToken);
        }


        [HttpPut("add-to-cart/{courseId}")]
        public async Task<IActionResult> AddToCartAsync(int courseId, CancellationToken cancellationToken)
        {
            await this._shoppingCartService.AddToCartAsync(courseId, Email, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._shoppingCartService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("buy")]
        public async Task<IActionResult> BuyAsync(CancellationToken cancellationToken)
        {
            await this._shoppingCartService.BuyAsync(Email, cancellationToken);
            return Ok();
        }
    }
}
