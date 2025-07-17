using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Entities.User;

public class UserTokenEntity: IdentityUserToken<Guid>, IEntity
{
    public UserEntity User { get; set; }
}