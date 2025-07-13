using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Infrastructure.Persistence.Configurations.User;

internal class RoleClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaimEntity>
{
    public void Configure(EntityTypeBuilder<RoleClaimEntity> builder)
    {
        builder.Property(c => c.ClaimType).HasMaxLength(1000);
        builder.Property(c => c.ClaimValue).HasMaxLength(1000);

        builder.ToTable("RoleClaims", "usr");
    }
}