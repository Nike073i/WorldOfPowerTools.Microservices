using WorldOfPowerTools.CartService.Data;
using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Services
{
    public class Cart
    {
        private readonly DbCartLineRepository _cartLineRepository;

        public Cart(DbCartLineRepository cartLineRepository)
        {
            _cartLineRepository = cartLineRepository;
        }

        public async Task<IEnumerable<CartLine>> GetUserProducts(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            return await _cartLineRepository.GetByUserIdAsync(userId);
        }

        public async Task<Cart> AddProduct(Guid userId, Guid productId, int count)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));

            int minQuantity = CartLine.MinProductQuantity;
            int maxQuantity = CartLine.MaxProductQuantity;
            if (count < minQuantity || count > maxQuantity) throw new ArgumentOutOfRangeException(nameof(count));

            int newQuantity = count;
            var userCartLines = await _cartLineRepository.GetByUserIdAsync(userId);
            var productCartLine = userCartLines.FirstOrDefault(cl => cl.ProductId == productId);
            if (productCartLine != null)
            {
                newQuantity += productCartLine.Quantity;
                newQuantity = newQuantity > maxQuantity ? maxQuantity : newQuantity;
            }
            productCartLine = new CartLine(userId, productId, newQuantity);
            await _cartLineRepository.SaveAsync(productCartLine);
            return this;
        }

        public async Task<Cart> RemoveProduct(Guid userId, Guid productId, int? count = null)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            if (count.HasValue && count.Value < 1) throw new ArgumentOutOfRangeException(nameof(count));

            var userCartLines = await _cartLineRepository.GetByUserIdAsync(userId);
            var productCartLine = userCartLines.FirstOrDefault(cl => cl.ProductId == productId);
            if (productCartLine == null) return this;

            if (!count.HasValue)
            {
                await _cartLineRepository.RemoveByIdAsync(productCartLine.Id!.Value);
                return this;
            }

            int newQuantity = productCartLine.Quantity - count.Value;
            if (newQuantity <= 0)
            {
                await _cartLineRepository.RemoveByIdAsync(productCartLine.Id!.Value);
                return this;
            }
            productCartLine.Quantity = newQuantity;
            await _cartLineRepository.SaveAsync(productCartLine);
            return this;
        }

        public async Task<int> Clear(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            return await _cartLineRepository.RemoveByUserIdAsync(userId);
        }
    }
}
