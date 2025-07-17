using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Entities.User;

public sealed class UserRoleEntity: IdentityUserRole<Guid>, IEntity
{
    public UserEntity User { get; set; }
    public RoleEntity Role { get; set; }
}