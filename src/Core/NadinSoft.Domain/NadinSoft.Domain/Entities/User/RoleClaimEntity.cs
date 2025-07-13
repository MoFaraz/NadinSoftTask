using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Entities.User;

public sealed class RoleClaimEntity : IdentityRoleClaim<Guid>, IEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public RoleEntity Role { get; set; }
}