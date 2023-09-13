using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

public interface IPlaceOrderV2Handler<in TInput> where TInput : IPlaceOrderV2Input
{
    public Task<Order> HandleAsync(TInput command, CancellationToken token);
}