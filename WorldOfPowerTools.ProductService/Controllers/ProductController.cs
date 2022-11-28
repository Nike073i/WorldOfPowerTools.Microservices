using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.ProductService.Data;
using WorldOfPowerTools.ProductService.Models;

namespace WorldOfPowerTools.ProductService.Controllers
{
    [Route("api/[controller]/")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbProductRepository _productRepository;

        public ProductController(DbProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            return Ok(await _productRepository.GetAllAsync(skip, count));
        }

        [HttpGet("category")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCategory([Required] Category category, int skip = 0, int? count = null)
        {
            return Ok(await _productRepository.GetByCategoryAsync(category, skip, count));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            return await _productRepository.GetByIdAsync(id) is { } item ? Ok(item) : NotFound("Продукт по указанному Id не найден");
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct([Required] string name, [Required] double price, [Required] string description, [Required] int quantity,
            Category category = Category.Screwdriver, bool availability = true)
        {
            try
            {
                var product = new Product(name, price, category, description, quantity, availability);
                var savedProduct = await _productRepository.SaveAsync(product);
                return Ok(savedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveProduct([Required] Guid id)
        {
            try
            {
                var productId = await _productRepository.RemoveByIdAsync(id);
                return Ok(productId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("loading")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProductToStore([Required] Guid productId, [Required] int quantity)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) return NotFound("Продукт по указанному Id не найден");
                product.AddToStore(quantity);
                var changedProduct = await _productRepository.SaveAsync(product);
                return Ok(changedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("unloading")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProductFromStore([Required] Guid productId, [Required] int quantity)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) return NotFound("Продукт по указанному Id не найден");
                product.RemoveFromStore(quantity);
                var changedProduct = await _productRepository.SaveAsync(product);
                return Ok(changedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([Required] Guid productId, string? name = null, double? price = null, string? description = null, Category? category = null)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) return NotFound("Продукт по указанному Id не найден");
                product.Name = name ?? product.Name;
                product.Price = price ?? product.Price;
                product.Description = description ?? product.Description;
                product.Category = category ?? product.Category;
                var changedProduct = await _productRepository.SaveAsync(product);
                return Ok(changedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
