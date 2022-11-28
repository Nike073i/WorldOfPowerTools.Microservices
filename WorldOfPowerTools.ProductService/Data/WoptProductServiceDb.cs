using Microsoft.EntityFrameworkCore;

namespace WorldOfPowerTools.ProductService.Data
{
    public class WoptProductServiceDb : DbContext
    {
        public WoptProductServiceDb(DbContextOptions<WoptProductServiceDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
