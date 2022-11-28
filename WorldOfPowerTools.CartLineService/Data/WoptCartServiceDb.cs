using Microsoft.EntityFrameworkCore;

namespace WorldOfPowerTools.CartService.Data
{
    public class WoptCartServiceDb : DbContext
    {
        public WoptCartServiceDb(DbContextOptions<WoptCartServiceDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CartLineConfiguration());
        }
    }
}
