using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.CartService.Proxies;
using WorldOfPowerTools.CartService.Services;

namespace WorldOfPowerTools.CartService.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IProductServiceProxy _productService;
        private readonly Cart _cart;

        public CartController(IProductServiceProxy productService, Cart cart)
        {
            _cart = cart;
            _productService = productService;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([Required] Guid userId)
        {
            return Ok(await _cart.GetUserProducts(userId));
        }

        /// TEST ///
        [HttpGet("test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Test([Required] Guid productId)
        {
            var token = HttpContext.Request.Headers.Authorization;
            await _productService.GetById(productId, token);
            return Ok();
        }
        /// TEST ///

        [HttpPut("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct([Required] Guid userId, [Required] Guid productId, [Required] int quantity)
        {
            try
            {
                await _cart.AddProduct(userId, productId, quantity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Продукт добавлен в корзину");
        }

        [HttpDelete("remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveProduct([Required] Guid userId, [Required] Guid productId, int? quantity = null)
        {
            try
            {
                await _cart.RemoveProduct(userId, productId, quantity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Удален из корзины");
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClearCart([Required] Guid userId)
        {
            try
            {
                await _cart.Clear(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Корзина очищена");
        }
    }
}
