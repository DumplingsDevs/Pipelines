namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.Dtos;

public class ProductDto
{
    public ProductDto(Guid id, int quantity)
    {
        Quantity = quantity;
        Id = id;
    }

    public Guid Id { get; }
    public int Quantity { get; }
}