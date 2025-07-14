namespace NadinSoft.Api.Models.Product;

public record CreateProductApiModel(
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate
);