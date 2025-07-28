namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByNameQueryResult(
    Guid Id,
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    bool Availability,
    DateTime ProduceDate,
    Guid UserId);