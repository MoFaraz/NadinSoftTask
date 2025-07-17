using Mediator;
using Microsoft.Extensions.Logging;
using NadinSoft.Domain.Events;

namespace NadinSoft.Application.Events;

public class ProductEditedDomainEventHandler(ILogger<ProductEditedDomainEventHandler> logger) : INotificationHandler<ProductEditedDomainEvent>
{
    public ValueTask Handle(ProductEditedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Product Edited Domain Event:  ProductId:{ProductId}, ProductName:{ProductName}, UserId:{UserId}",
            notification.ProductId, notification.ProductName, notification.UserId);
        return ValueTask.CompletedTask;
    }
}