using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Entities.User;

public sealed class RoleClaimEntity : IdentityRoleClaim<Guid>, IEntity
{
    public RoleEntity Role { get; set; }
}