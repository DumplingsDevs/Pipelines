using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.Repositories.Orders;

public interface IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken cancellationToken);
}