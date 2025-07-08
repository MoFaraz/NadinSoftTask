using Ardalis.GuardClauses;

namespace NadinSoft.Domain.Entities.Product;

public class ProductEntity
{
    public Guid? Id { get; private init; }
    public string Name { get; private set; } = string.Empty;
    public string ManufacturePhone { get; private set; } = string.Empty;
    public string ManufactureEmail { get; private set; } = string.Empty;
    public DateTime ProduceDate { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid? UserId { get; private set; }

    private ProductEntity()
    {
    }

    public static ProductEntity Create(Guid? id, string name, string manufacturePhone, string manufactureEmail,
        DateTime produceDate, Guid? userId)
    {
        Guard.Against.NullOrEmpty(id, message: "Id cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(name, message: "Name cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufacturePhone, message: "ManufacturePhone cannot be null or empty.");
        Guard.Against.NullOrWhiteSpace(manufactureEmail, message: "ManufactureEmail cannot be null or empty.");
        Guard.Against.NullOrEmpty(userId, message: "UserId cannot be null or empty.");

        return new ProductEntity()
        {
            Id = id,
            Name = name,
            IsAvailable = true,
            ManufacturePhone = manufacturePhone,
            ManufactureEmail = manufactureEmail,
            ProduceDate = produceDate,
            UserId = userId
        };
    }

    public void ChangeAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public override bool Equals(object? product)
    {
        if (product is null)
            return false;

        if (product is not ProductEntity productEntity)
            return false;

        if (ReferenceEquals(this, product))
            return true;

        return productEntity.Id.Equals(this.Id);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}