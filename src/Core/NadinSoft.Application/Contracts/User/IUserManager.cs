using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Application.Contracts.User;

public interface IUserManager
{
    Task<IdentityResult> PasswordCreateAsync(UserEntity user, string password, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IdentityResult> ValidatePasswordAsync(UserEntity user, string passwordHash,
        CancellationToken cancellationToken = default);
}