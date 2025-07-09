namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByNameQueryResult(Guid Id, string ManufacturePhone, string ManufactureEmail, DateTime ProduceDate, Guid UserId);