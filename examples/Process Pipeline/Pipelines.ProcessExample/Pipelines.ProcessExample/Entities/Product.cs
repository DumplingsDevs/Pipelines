namespace Pipelines.ProcessExample.Entities;

public class Product
{
    public Product(Guid productId, int quantity, decimal pricePerItem)
    {
        Quantity = quantity;
        ProductId = productId;
        PricePerItem = pricePerItem;
    }

    public Guid ProductId { get; }
    public int Quantity { get; }
    public decimal PricePerItem { get; }
}