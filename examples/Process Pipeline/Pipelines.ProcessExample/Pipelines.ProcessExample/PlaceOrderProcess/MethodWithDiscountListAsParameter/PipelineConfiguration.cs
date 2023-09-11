using System.Reflection;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter;

public static class PipelineConfiguration
{
    public static void AddPipelineWithDiscountAsMethodParameter(this IServiceCollection services, Assembly processAssembly)
    {
        services.AddPipeline()
            .AddInput(typeof(IPlaceOrderInput))
            .AddHandler(typeof(IPlaceOrderHandler<>), processAssembly)
            .AddDispatcher<IPlaceOrderDispatcher>(processAssembly)
            .WithDecorators(x =>
                x.WithAttribute<OrderStep>().OrderBy(z => z.StepNumber))
            .Build();
    }
}