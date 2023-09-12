namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Dtos;

public class ProductDtoV2
{
    public ProductDtoV2(Guid id, int quantity)
    {
        Quantity = quantity;
        Id = id;
    }

    public Guid Id { get; }
    public int Quantity { get; }
}