using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleEventHandler : 
    INotificationHandler<SaleCreatedEvent>,
    INotificationHandler<SaleModifiedEvent>,
    INotificationHandler<SaleCancelledEvent>,
    INotificationHandler<ItemCancelledEvent>
{
    private readonly ILogger<SaleEventHandler> _logger;

    public SaleEventHandler(ILogger<SaleEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Published: SaleCreated for SaleId: {SaleId} - Customer: {Customer}", notification.Sale.Id, notification.Sale.Customer);
        return Task.CompletedTask;
    }

    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Published: SaleModified for SaleId: {SaleId}", notification.Sale.Id);
        return Task.CompletedTask;
    }

    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Published: SaleCancelled for SaleId: {SaleId}", notification.SaleId);
        return Task.CompletedTask;
    }

    public Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Published: ItemCancelled for SaleId: {SaleId}, ItemId: {ItemId}", notification.SaleId, notification.ItemId);
        return Task.CompletedTask;
    }
}
