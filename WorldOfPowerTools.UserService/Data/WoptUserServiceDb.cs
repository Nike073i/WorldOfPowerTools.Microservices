using Microsoft.EntityFrameworkCore;

namespace WorldOfPowerTools.UserService.Data
{
    public class WoptUserServiceDb : DbContext
    {
        public WoptUserServiceDb(DbContextOptions<WoptUserServiceDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
