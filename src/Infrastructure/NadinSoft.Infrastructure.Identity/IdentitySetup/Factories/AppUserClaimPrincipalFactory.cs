using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Infrastructure.Identity.IdentitySetup.Factories;

internal class AppUserClaimPrincipalFactory(
    UserManager<UserEntity> userManager,
    RoleManager<RoleEntity> roleManager,
    IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<UserEntity, RoleEntity>(userManager, roleManager, options)
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserEntity user)
    {
        
        var claimsIdentity = await base.GenerateClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);
        
        foreach (var userRole in userRoles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userRole));
        }
        
        claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, user.UserCode));

        return claimsIdentity;
    }
}