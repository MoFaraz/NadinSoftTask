using Mediator;
using Microsoft.Extensions.Logging;
using NadinSoft.Domain.Events;

namespace NadinSoft.Application.Events;

public class ProductCreatedDomainEventHandler(ILogger<ProductCreatedDomainEventHandler> logger)
    : INotificationHandler<ProductCreatedDomainEvent>
{
    public ValueTask Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Product Created Domain Event:  ProductId:{ProductId}, ProductName:{ProductName}, UserId:{UserId}",
            notification.ProductId, notification.ProductName, notification.UserId);
        
        return ValueTask.CompletedTask;
    }
}