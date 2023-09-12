using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.Products;

public class GetProducts : IGetProducts
{
    public List<Product> Get(List<Guid> ids)
    {
        return new List<Product>();
    }
}