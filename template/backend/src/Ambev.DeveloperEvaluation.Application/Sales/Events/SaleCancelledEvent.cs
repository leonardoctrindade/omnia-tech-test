using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleCancelledEvent : INotification
{
    public Guid SaleId { get; }

    public SaleCancelledEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}
