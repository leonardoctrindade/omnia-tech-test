using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }

    public SaleItem() { }

    public SaleItem(string product, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        IsCancelled = false;
        
        CalculateDiscountAndTotal();
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    public void CalculateDiscountAndTotal()
    {
        if (Quantity < 4)
        {
            Discount = 0;
        }
        else if (Quantity >= 4 && Quantity < 10)
        {
            Discount = 0.10m; // 10%
        }
        else if (Quantity >= 10 && Quantity <= 20)
        {
            Discount = 0.20m; // 20%
        }
        else
        {
            throw new InvalidOperationException("Cannot sell more than 20 identical items.");
        }

        var grossAmount = Quantity * UnitPrice;
        var discountAmount = grossAmount * Discount;
        TotalAmount = grossAmount - discountAmount;
    }
}
