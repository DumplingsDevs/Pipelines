using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

public interface IPlaceOrderV2Dispatcher
{
    public Task<Order> SendAsync(IPlaceOrderV2Input command, CancellationToken token);
}