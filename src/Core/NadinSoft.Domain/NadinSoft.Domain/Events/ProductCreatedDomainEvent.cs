using NadinSoft.Domain.Common;

namespace NadinSoft.Domain.Events;

public sealed record ProductCreatedDomainEvent(Guid ProductId, string ProductName, Guid UserId) : IDomainEvent 
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}