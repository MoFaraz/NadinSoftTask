using NadinSoft.Application.Contracts.User.Models;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Application.Contracts.User;

public interface IJwtService
{
    Task<JwtAccessTokenModel> GenerateTokenAsync(UserEntity user, CancellationToken cancellationToken=default);
}