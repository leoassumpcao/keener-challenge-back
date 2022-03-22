using API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Mapping
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(user => user.Name)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(user => user.BirthDate)
                .HasColumnType("DATE")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.OwnsOne(user => user.Address, address =>
            {
                address.Property(address => address.AddressLine1)
                    .HasColumnName("AddressLine1")
                    .HasMaxLength(128)
                    .IsRequired();

                address.Property(address => address.AddressLine2)
                    .HasColumnName("AddressLine2")
                    .HasMaxLength(128)
                    .IsRequired();

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

            builder.Property(user => user.IsActive)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(user => user.MemberSince)
                .HasColumnType("DATE")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasMany(user => user.Orders)
                .WithOne(order => order.User);
        }
    }
}
