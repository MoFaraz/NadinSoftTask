using Mediator;
using Microsoft.Extensions.Logging;
using NadinSoft.Domain.Events;
using Serilog;

namespace NadinSoft.Application.Events;

public class ChangeAvailabilityDomainEventHandler(ILogger<ChangeAvailabilityDomainEventHandler> logger) : INotificationHandler<ChangeAvailabilityDomainEvent>
{
    public ValueTask Handle(ChangeAvailabilityDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Change Availability Domain Event:  ProductId:{ProductId}, ProductName:{ProductName}, UserId:{UserId}",
            notification.ProductId, notification.ProductName, notification.UserId);
        return ValueTask.CompletedTask;
    }
}