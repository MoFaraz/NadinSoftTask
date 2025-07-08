using Microsoft.AspNetCore.Identity;
using NadinSoft.Domain.Common;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Domain.Entities.User;

public sealed class UserEntity: IdentityUser<Guid>, IEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    private List<ProductEntity> _products = [];
    
    public IReadOnlyList<ProductEntity> Products => _products;

    public UserEntity(string firstName, string lastName, string userName, string email) : base(userName)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}