using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.Repositories.Orders;

public class OrderRepository : IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}