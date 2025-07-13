using Microsoft.AspNetCore.Identity;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Infrastructure.Identity.Services.Implementations;

internal class UserManagerImplementation(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
    : IUserManager
{
    public async Task<IdentityResult> PasswordCreateAsync(UserEntity user, string password,
        CancellationToken cancellationToken = default)
    {
        return await userManager.CreateAsync(user, password);
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName,
        CancellationToken cancellationToken = default)
    {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> ValidatePasswordAsync(UserEntity user, string givenPassword,
        CancellationToken cancellationToken = default)
    {
        var checkPassword = await signInManager.CheckPasswordSignInAsync(user, givenPassword, true);
        
        if(checkPassword.Succeeded)
            return IdentityResult.Success;
        
        return IdentityResult.Failed(new IdentityError(){Code = "InvalidPassword", Description = "Password is incorrect."});
    }
}