using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Domain.Entities.User;

public sealed class UserEntity : IdentityUser<Guid>, IEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public string UserCode { get; private set; }
    private List<ProductEntity> _products = [];

    public IReadOnlyList<ProductEntity> Products => _products;

    public UserEntity(string firstName, string lastName, string userName, string email) : base(userName)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserCode = Guid.NewGuid().ToString("N")[0..7];
    }

    public ICollection<UserRoleEntity> UserRoles { get; set; }
    public ICollection<UserClaimEntity> UserClaims { get; set; }
    public ICollection<UserLoginEntity> UserLogins { get; set; }
    public ICollection<UserTokenEntity> UserTokens { get; set; }
}