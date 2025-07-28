namespace NadinSoft.Api.Models.Product;

public record SearchProductModel(string SearchTerm, int Page = 1, int PageSize = 10);