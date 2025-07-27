namespace NadinSoft.Api.Models.Product;

public record EditProductApiModel(
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate
);