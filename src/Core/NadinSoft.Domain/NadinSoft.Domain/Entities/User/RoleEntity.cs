using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Entities.User;

public sealed class RoleEntity: IdentityRole<Guid>, IEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public string DisplayName { get; private set; }

    public RoleEntity(string displayName, string name): base(name)
    {
        DisplayName = displayName;
    }
    
    
    public ICollection<UserRoleEntity> UserRoles { get; set; }
    public ICollection<RoleClaimEntity> RoleClaims { get; set; }
}