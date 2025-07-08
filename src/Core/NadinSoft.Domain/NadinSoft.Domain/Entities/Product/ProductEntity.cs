using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Entities.Product;

public sealed partial class ProductEntity: BaseEntity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string ManufacturePhone { get; private set; } = string.Empty;
    public string ManufactureEmail { get; private set; } = string.Empty;
    public DateTime ProduceDate { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid? UserId { get; private set; }
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

    public static ProductEntity Create(Guid id, string name, string manufacturePhone, string manufactureEmail,
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