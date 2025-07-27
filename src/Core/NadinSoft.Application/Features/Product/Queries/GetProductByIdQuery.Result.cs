namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByIdQueryResult(
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    bool IsAvailable,
    string Username,
    DateTime ProduceDate
);