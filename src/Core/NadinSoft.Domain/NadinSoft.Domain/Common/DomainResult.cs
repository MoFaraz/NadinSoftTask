namespace NadinSoft.Domain.Common;

public record DomainResult(bool IsSuccess, string? ErrorMessage);