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
        private readonly IUserServiceProxy _userService;
        private readonly Cart _cart;

        public CartController(IProductServiceProxy productService, IUserServiceProxy userService, Cart cart)
        {
            _cart = cart;
            _productService = productService;
            _userService = userService;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([Required] Guid userId)
        {
            return Ok(await _cart.GetUserProducts(userId));
        }

        [HttpPut("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProduct([Required] Guid userId, [Required] Guid productId, [Required] int quantity)
        {
            var token = HttpContext.Request.Headers.Authorization;
            try
            {
                var user = await _userService.GetById(userId, token);
                if (user == null) return NotFound("Пользователь с указанным Id не найден");
                var product = await _productService.GetById(productId, token);
                if (product == null || !product.Availability) return NotFound("Продукт с указанным Id не найден или недоступен");
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProduct([Required] Guid userId, [Required] Guid productId, int? quantity = null)
        {
            var token = HttpContext.Request.Headers.Authorization;
            try
            {
                var user = await _userService.GetById(userId, token);
                if (user == null) return NotFound("Пользователь с указанным Id не найден");
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearCart([Required] Guid userId)
        {
            var token = HttpContext.Request.Headers.Authorization;
            try
            {
                var user = await _userService.GetById(userId, token);
                if (user == null) return NotFound("Пользователь с указанным Id не найден");
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
