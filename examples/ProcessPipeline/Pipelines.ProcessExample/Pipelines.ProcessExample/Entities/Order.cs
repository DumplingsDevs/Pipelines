namespace Pipelines.ProcessExample.Entities;

public class Order
{
    public Order(List<Product> products, List<Discount> discounts)
    {
        Id = Guid.NewGuid();
        Products = products;
        Discounts = discounts;
    }

    public Guid Id { get; }
    public List<Product> Products { get; }
    public List<Discount> Discounts { get; }
}