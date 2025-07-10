using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Infrastructure.Persistence.Configurations.Product;

internal class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(60);
        builder.Property(c => c.ManufacturePhone).HasMaxLength(13);
        builder.Property(c => c.ManufactureEmail).HasMaxLength(60);
        builder.Property(c => c.IsAvailable)
            .HasConversion(new BoolToStringConverter("False", "True"))
            .HasMaxLength(5);
        builder.HasOne(c => c.User)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.UserId);



        builder.HasIndex(c => c.Name);
        builder
            .HasIndex(c => new { c.ManufactureEmail, c.ProduceDate })
            .IsUnique();

        builder.ToTable("Products", "product");
    }
}