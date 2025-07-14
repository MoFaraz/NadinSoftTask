namespace NadinSoft.Api.Models.Product;

public record EditProductApiModel(
    Guid Id,
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate
);