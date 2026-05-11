using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    private readonly Faker _faker = new("en");

    [Fact(DisplayName = "Given quantity below 4 When calculating discount Then should apply 0% discount")]
    public void CalculateDiscount_QuantityBelow4_Applies0PercentDiscount()
    {
        // Arrange
        var unitPrice = 100m;
        var quantity = 3;
        
        // Act
        var saleItem = new SaleItem("Product A", quantity, unitPrice);

        // Assert
        Assert.Equal(0m, saleItem.Discount);
        Assert.Equal(300m, saleItem.TotalAmount);
    }

    [Fact(DisplayName = "Given quantity between 4 and 9 When calculating discount Then should apply 10% discount")]
    public void CalculateDiscount_QuantityBetween4And9_Applies10PercentDiscount()
    {
        // Arrange
        var unitPrice = 100m;
        var quantity = 5; // 500 total, 10% discount = 50, final = 450
        
        // Act
        var saleItem = new SaleItem("Product A", quantity, unitPrice);

        // Assert
        Assert.Equal(0.10m, saleItem.Discount);
        Assert.Equal(450m, saleItem.TotalAmount);
    }

    [Fact(DisplayName = "Given quantity between 10 and 20 When calculating discount Then should apply 20% discount")]
    public void CalculateDiscount_QuantityBetween10And20_Applies20PercentDiscount()
    {
        // Arrange
        var unitPrice = 100m;
        var quantity = 15; // 1500 total, 20% discount = 300, final = 1200
        
        // Act
        var saleItem = new SaleItem("Product A", quantity, unitPrice);

        // Assert
        Assert.Equal(0.20m, saleItem.Discount);
        Assert.Equal(1200m, saleItem.TotalAmount);
    }

    [Fact(DisplayName = "Given quantity above 20 When creating item Then should throw InvalidOperationException")]
    public void CalculateDiscount_QuantityAbove20_ThrowsException()
    {
        // Arrange
        var unitPrice = 100m;
        var quantity = 21;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => new SaleItem("Product A", quantity, unitPrice));
        Assert.Equal("Cannot sell more than 20 identical items.", exception.Message);
    }

    [Fact(DisplayName = "Given an item When cancelled Then IsCancelled should be true")]
    public void Cancel_WhenCalled_SetsIsCancelledToTrue()
    {
        // Arrange
        var saleItem = new SaleItem("Product A", 1, 100m);

        // Act
        saleItem.Cancel();

        // Assert
        Assert.True(saleItem.IsCancelled);
    }
}
