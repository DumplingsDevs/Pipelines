using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.Products;

public interface IGetProducts
{
    public List<Product> Get(List<Guid> ids);
}