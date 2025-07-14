namespace NadinSoft.Application.Features.Product.Queries;

public record GetAllProductsQueryResult(
    Guid Id,
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    bool Availability,
    DateTime ProduceDate
);