using Mediator;

namespace NadinSoft.Domain.Common;

public interface IDomainEvent: INotification
{
    public DateTime OccurredOn { get; }
}