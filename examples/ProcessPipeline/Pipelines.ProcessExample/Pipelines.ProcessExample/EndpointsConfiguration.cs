using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

namespace Pipelines.ProcessExample;

public static class EndpointsConfiguration
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapPost("/order", async (PlaceOrder command, IPlaceOrderDispatcher placeOrderDispatcher, CancellationToken token) =>
        {
            var result = await placeOrderDispatcher.SendAsync(command, new List<Discount>(), token);
            return Results.Ok(result);
        });

        app.MapPost("v2/order", async (PlaceOrderV2 command, IPlaceOrderV2Dispatcher placeOrderDispatcher, CancellationToken token) =>
        {
            var result = await placeOrderDispatcher.SendAsync(command, token);
            return Results.Ok(result);
        });
    }
}