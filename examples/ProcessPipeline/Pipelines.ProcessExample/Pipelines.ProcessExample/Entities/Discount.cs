namespace Pipelines.ProcessExample.Entities;

public class Discount
{
    public Discount(Guid productId, string discountType, decimal discountValue)
    {
        ProductId = productId;
        DiscountType = discountType;
        DiscountValue = discountValue;
    }

    public Guid ProductId { get; }
    public string DiscountType { get; }
    public decimal DiscountValue { get; }
}