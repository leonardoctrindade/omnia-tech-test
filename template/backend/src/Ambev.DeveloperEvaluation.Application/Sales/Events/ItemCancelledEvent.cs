using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class ItemCancelledEvent : INotification
{
    public Guid SaleId { get; }
    public Guid ItemId { get; }

    public ItemCancelledEvent(Guid saleId, Guid itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }
}
