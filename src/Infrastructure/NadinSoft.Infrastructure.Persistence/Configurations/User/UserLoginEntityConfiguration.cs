using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Infrastructure.Persistence.Configurations.User;

internal class UserLoginEntityConfiguration: IEntityTypeConfiguration<UserLoginEntity>
{
    public void Configure(EntityTypeBuilder<UserLoginEntity> builder)
    {
        builder.HasKey(c => new {c.LoginProvider, c.ProviderKey});
        
        builder.Property(c => c.LoginProvider).HasMaxLength(100);
        builder.Property(c => c.ProviderKey).HasMaxLength(500);
        builder.Property(c => c.ProviderDisplayName).HasMaxLength(100);
        
        builder.ToTable("UserLogins", "usr");
        
    }
}