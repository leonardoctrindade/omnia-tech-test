using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public decimal TotalAmount { get; private set; }
    public SaleStatus Status { get; set; }
    
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public Sale() 
    {
        Date = DateTime.UtcNow;
        Status = SaleStatus.Active;
    }

    public void AddItem(SaleItem item)
    {
        if (Status == SaleStatus.Cancelled)
            throw new InvalidOperationException("Cannot add items to a cancelled sale.");

        // Check if adding this item exceeds the 20 item limit for the same product
        var existingQuantity = _items.Where(i => i.Product == item.Product && !i.IsCancelled).Sum(i => i.Quantity);
        if (existingQuantity + item.Quantity > 20)
            throw new InvalidOperationException($"Cannot sell more than 20 identical items. Product: {item.Product}");

        item.SaleId = Id;
        _items.Add(item);
        
        CalculateTotal();
    }

    public void CancelItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item not found in this sale.");

        item.Cancel();
        CalculateTotal();
    }

    public void CancelSale()
    {
        Status = SaleStatus.Cancelled;
        // Optionally cancel all items
        foreach (var item in _items)
        {
            item.Cancel();
        }
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        TotalAmount = _items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
    }
}
