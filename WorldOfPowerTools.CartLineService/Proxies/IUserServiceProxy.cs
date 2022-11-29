using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Proxies
{
    public interface IUserServiceProxy
    {
        public Task<User?> GetById(Guid userId, string token);
    }
}
