namespace NadinSoft.Application.Features.Product.Queries;

public record GetAllProductsQueryResult(
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate
);