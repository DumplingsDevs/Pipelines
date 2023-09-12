using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;

public interface IPlaceOrderDispatcher
{
    public Task<Order> SendAsync(IPlaceOrderInput command, List<Discount> discounts, CancellationToken token);
}