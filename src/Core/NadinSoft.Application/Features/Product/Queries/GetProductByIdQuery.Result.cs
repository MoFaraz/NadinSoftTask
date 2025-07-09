namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByIdQueryResult(string Name, string ManufacturePhone, string ManufactureEmail, DateTime ProduceDate, Guid UserId);