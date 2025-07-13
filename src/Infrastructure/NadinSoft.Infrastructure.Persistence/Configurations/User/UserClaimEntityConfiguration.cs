using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Infrastructure.Persistence.Configurations.User;

internal class UserClaimEntityConfiguration: IEntityTypeConfiguration<UserClaimEntity>
{
    public void Configure(EntityTypeBuilder<UserClaimEntity> builder)
    {
        builder.Property(c => c.ClaimType).HasMaxLength(1000);
        builder.Property(c => c.ClaimValue).HasMaxLength(1000);

        builder.HasKey(c => c.Id);
        
        builder.ToTable("UserClaims", "usr");
    }
}