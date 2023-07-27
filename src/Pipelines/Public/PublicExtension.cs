using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder;
using Pipelines.Builder.Interfaces;

namespace Pipelines.Public;

public static class Extension
{
    public static IInputBuilder AddPipeline(this IServiceCollection service)
    {
        return new PipelineBuilder(service);
    }
}