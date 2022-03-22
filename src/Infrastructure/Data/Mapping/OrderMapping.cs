using API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Mapping
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(pur => pur.Id);

            builder.Property(pur => pur.UserId)
                .IsRequired();

            builder.Property(pur => pur.TotalPaid)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.Property(pur => pur.Total)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.Property(pur => pur.CreatedAt)
                .IsRequired();

            builder.OwnsOne(user => user.DeliveryAddress, address =>
            {
                address.Property(address => address.AddressLine1)
                    .HasColumnName("AddressLine1")
                    .HasMaxLength(128)
                    .IsRequired();

                address.Property(address => address.AddressLine2)
                    .HasColumnName("AddressLine2")
                    .HasMaxLength(128)
                    .IsRequired(false);

                address.Property(address => address.State)
                    .HasColumnName("AddressState")
                    .HasMaxLength(64)
                    .IsRequired();

                address.Property(address => address.City)
                    .HasColumnName("AddressCity")
                    .HasMaxLength(64)
                    .IsRequired();

                address.Property(address => address.Neighborhood)
                    .HasColumnName("AddressNeighborhood")
                    .HasMaxLength(128)
                    .IsRequired();

                address.Property(address => address.Country)
                    .HasColumnName("AddressCountry")
                    .HasMaxLength(128)
                    .IsRequired();

                address.Property(address => address.ZipCode)
                    .HasColumnName("AddressZipCode")
                    .HasMaxLength(64)
                    .IsRequired();
            });

            builder.HasMany<OrderedProduct>(s => s.OrderedProducts)
                .WithOne(op => op.Order);
        }
    }
}
