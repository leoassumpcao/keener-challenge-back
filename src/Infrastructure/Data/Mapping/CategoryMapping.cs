using API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Data.Mapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(qn => qn.Id);

            builder.Property(qn => qn.Name)
                .IsRequired();

            builder.HasMany<Product>(c => c.Products)
                .WithOne(p => p.Category);
        }
    }
}
