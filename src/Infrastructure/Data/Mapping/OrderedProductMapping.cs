using API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Mapping
{
    public class OrderedProductMapping : IEntityTypeConfiguration<OrderedProduct>
    {
        public void Configure(EntityTypeBuilder<OrderedProduct> builder)
        {
            builder.ToTable("ordered_products");
            builder.HasKey(pur => pur.Id);

            builder.Property(pur => pur.OrderId)
                .IsRequired();

            builder.Property(pur => pur.ProductId)
                .IsRequired();

            builder.Property(pur => pur.Quantity)
                .IsRequired();

            builder.Property(pur => pur.UnitPrice)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.HasOne(s => s.Product)
                .WithMany();

            builder.HasOne(s => s.Order)
                .WithMany(o => o.OrderedProducts);

        }
    }
}
