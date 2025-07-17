using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using NadinSoft.Domain.Common;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Domain.Events;

namespace NadinSoft.Domain.Entities.Product;

public sealed partial class ProductEntity : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public string ManufacturePhone { get; private set; } = string.Empty;
    public string ManufactureEmail { get; private set; } = string.Empty;
    public DateTime ProduceDate { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid UserId { get; private set; }
    public string Slug => GenerateSlug();

    private string GenerateSlug()
    {
        var name = SlugRegex().Replace(Name, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{name}-{ProduceDate:yyyy-MM-dd}";
    }


    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();

    private ProductEntity()
    {
    }

    #region Navigation

    public UserEntity User { get; private set; }

    #endregion

    public static ProductEntity Create(string name, string manufacturePhone, string manufactureEmail,
        DateTime produceDate, Guid userId)
    {
        Guard.Against.NullOrWhiteSpace(name, message: "Name cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufacturePhone, message: "ManufacturePhone cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufactureEmail, message: "ManufactureEmail cannot be null or empty.");
        Guard.Against.NullOrEmpty(userId, message: "UserId cannot be null or empty.");

        var product = new ProductEntity()
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsAvailable = true,
            ManufacturePhone = manufacturePhone,
            ManufactureEmail = manufactureEmail,
            ProduceDate = produceDate,
            UserId = userId
        };
        product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.UserId));
        return product;
    }

    public static ProductEntity Create(string name, string manufacturePhone, string manufactureEmail,
        DateTime produceDate, UserEntity user)
    {
        Guard.Against.NullOrWhiteSpace(name, message: "Name cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufacturePhone, message: "ManufacturePhone cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufactureEmail, message: "ManufactureEmail cannot be null or empty.");
        Guard.Against.Null(user, message: "User cannot be null.");

        var product = new ProductEntity()
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsAvailable = true,
            ManufacturePhone = manufacturePhone,
            ManufactureEmail = manufactureEmail,
            ProduceDate = produceDate,
            UserId = user.Id,
            User = user
        };

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.UserId));
        return product;
    }

    public static ProductEntity Create(Guid id, string name, string manufacturePhone, string manufactureEmail,
        DateTime produceDate, Guid userId)
    {
        Guard.Against.NullOrEmpty(id, message: "Id cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(name, message: "Name cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufacturePhone, message: "ManufacturePhone cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufactureEmail, message: "ManufactureEmail cannot be null or empty.");
        Guard.Against.NullOrEmpty(userId, message: "UserId cannot be null or empty.");

        var product = new ProductEntity()
        {
            Id = id,
            Name = name,
            IsAvailable = true,
            ManufacturePhone = manufacturePhone,
            ManufactureEmail = manufactureEmail,
            ProduceDate = produceDate,
            UserId = userId
        };

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.UserId));

        return product;
    }

    public void Edit(string name, string manufacturePhone, string manufactureEmail,
        DateTime produceDate)
    {
        Guard.Against.NullOrWhiteSpace(name, message: "Name cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufacturePhone, message: "ManufacturePhone cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufactureEmail, message: "ManufactureEmail cannot be null or empty.");

        Name = name;
        ManufacturePhone = manufacturePhone;
        ManufactureEmail = manufactureEmail;
        ProduceDate = produceDate;

        RaiseDomainEvent(new ProductEditedDomainEvent(Id, Name, UserId));
    }

    public void ChangeAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;

        RaiseDomainEvent(new ChangeAvailabilityDomainEvent(Id, Name, UserId));
    }

    public override bool Equals(object? product)
    {
        if (product is null)
            return false;

        if (product is not ProductEntity productEntity)
            return false;

        if (ReferenceEquals(this, product))
            return true;

        return productEntity.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}