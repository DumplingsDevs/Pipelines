using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;

public interface IPlaceOrderHandler<in TInput> where TInput : IPlaceOrderInput
{
    public Task<Order> HandleAsync(TInput command, List<Discount> discounts, CancellationToken token);
}