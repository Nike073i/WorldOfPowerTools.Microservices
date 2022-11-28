using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.ProductService.Models;

namespace WorldOfPowerTools.ProductService.Data
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public virtual void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
        }
    }
}