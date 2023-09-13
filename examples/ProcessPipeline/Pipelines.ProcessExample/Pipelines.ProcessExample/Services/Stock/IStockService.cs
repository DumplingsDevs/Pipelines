namespace Pipelines.ProcessExample.Services.Stock;

public interface IStockService
{
    public bool CheckIfInStock(Guid productId, int quantity);
}