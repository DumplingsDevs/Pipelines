using System.Reflection;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState;

public static class PipelineConfiguration
{
    public static void AddPipelineWithSharedState(this IServiceCollection services, Assembly processAssembly)
    {
        services.AddPipeline()
            .AddInput(typeof(IPlaceOrderV2Input))
            .AddHandler(typeof(IPlaceOrderV2Handler<>), processAssembly)
            .AddDispatcher<IPlaceOrderV2Dispatcher>(processAssembly)
            .WithDecorators(x =>
                x.WithAttribute<OrderStepV2>().OrderBy(z => z.StepNumber), processAssembly)
            .Build();

        services.AddScoped<DiscountState>();
    }
}