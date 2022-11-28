using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Data
{
    public class CartLineConfiguration : IEntityTypeConfiguration<CartLine>
    {
        public virtual void Configure(EntityTypeBuilder<CartLine> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}