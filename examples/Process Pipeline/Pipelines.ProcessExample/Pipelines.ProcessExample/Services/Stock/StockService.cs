namespace Pipelines.ProcessExample.Services.Stock;

public class StockService : IStockService
{
    public bool CheckIfInStock(Guid productId, int quantity)
    {
        return true;
    }
}