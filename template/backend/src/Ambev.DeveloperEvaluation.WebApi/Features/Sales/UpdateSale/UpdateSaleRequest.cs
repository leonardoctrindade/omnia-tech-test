namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
    public string SaleNumber { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
}
