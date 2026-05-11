using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Given valid items When adding to sale Then should calculate total amount correctly")]
    public void AddItem_ValidItems_CalculatesTotalAmount()
    {
        // Arrange
        var sale = new Sale { SaleNumber = "123", Customer = "John Doe", Branch = "Main Branch" };
        var item1 = new SaleItem("Product A", 3, 100m); // 300
        var item2 = new SaleItem("Product B", 5, 100m); // 500 - 10% = 450

        // Act
        sale.AddItem(item1);
        sale.AddItem(item2);

        // Assert
        Assert.Equal(2, sale.Items.Count);
        Assert.Equal(750m, sale.TotalAmount);
    }

    [Fact(DisplayName = "Given items of same product When total exceeds 20 Then should throw InvalidOperationException")]
    public void AddItem_Exceeding20ItemsForSameProduct_ThrowsException()
    {
        // Arrange
        var sale = new Sale { SaleNumber = "123" };
        var item1 = new SaleItem("Product A", 15, 100m);
        var item2 = new SaleItem("Product A", 6, 100m); // 15 + 6 = 21 (exceeds limit)

        // Act
        sale.AddItem(item1);
        
        // Assert
        var exception = Assert.Throws<InvalidOperationException>(() => sale.AddItem(item2));
        Assert.Contains("Cannot sell more than 20 identical items", exception.Message);
    }

    [Fact(DisplayName = "Given a cancelled sale When adding item Then should throw InvalidOperationException")]
    public void AddItem_ToCancelledSale_ThrowsException()
    {
        // Arrange
        var sale = new Sale { SaleNumber = "123" };
        sale.CancelSale();
        var item = new SaleItem("Product A", 1, 100m);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => sale.AddItem(item));
        Assert.Equal("Cannot add items to a cancelled sale.", exception.Message);
    }

    [Fact(DisplayName = "Given an item in sale When cancelling item Then should update sale total amount")]
    public void CancelItem_ExistingItem_UpdatesTotalAmount()
    {
        // Arrange
        var sale = new Sale { SaleNumber = "123" };
        var item1 = new SaleItem("Product A", 1, 100m);
        var item2 = new SaleItem("Product B", 1, 200m);
        sale.AddItem(item1);
        sale.AddItem(item2);

        // Act
        sale.CancelItem(item1.Id);

        // Assert
        Assert.True(item1.IsCancelled);
        Assert.Equal(200m, sale.TotalAmount); // Only Product B counts now
    }

    [Fact(DisplayName = "Given an active sale When cancelled Then status should be Cancelled and items cancelled")]
    public void CancelSale_ActiveSale_CancelsSaleAndItems()
    {
        // Arrange
        var sale = new Sale { SaleNumber = "123" };
        var item = new SaleItem("Product A", 1, 100m);
        sale.AddItem(item);

        // Act
        sale.CancelSale();

        // Assert
        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.True(item.IsCancelled);
        Assert.Equal(0m, sale.TotalAmount); // Since all items are cancelled
    }
}
