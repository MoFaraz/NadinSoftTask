using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Application.Contracts.User;

public interface IUserManager
{
    Task<IdentityResult> PasswordCreateAsync(UserEntity user, string password, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IdentityResult> ValidatePasswordAsync(UserEntity user, string givenPassword,
        CancellationToken cancellationToken = default);
}