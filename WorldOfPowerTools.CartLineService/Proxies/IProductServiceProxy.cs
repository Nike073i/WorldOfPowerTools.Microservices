using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Proxies
{
    public interface IProductServiceProxy
    {
        public Task<Product> GetById(Guid productId, string token);
    }
}
