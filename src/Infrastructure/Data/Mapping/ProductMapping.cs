using API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Mapping
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired(false);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.Property(p => p.CategoryId)
                .IsRequired(false);

            builder.HasOne<Category>(p => p.Category)
                .WithMany(c => c.Products)
                .IsRequired(false);

            builder.HasMany(p => p.Images)
                .WithOne(c => c.Product)
                .IsRequired(false);

        }
    }
}
