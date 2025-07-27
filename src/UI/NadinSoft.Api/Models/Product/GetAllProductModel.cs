namespace NadinSoft.Api.Models.Product;

public record GetAllProductModel(string? Username, int Page = 1, int PageSize = 10);