using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Infrastructure.Persistence.Configurations.User;

internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles", "usr");

        builder.HasKey(c => new { c.UserId, c.RoleId });
    }
}