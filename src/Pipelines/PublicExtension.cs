using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder;
using Pipelines.Builder.Interfaces;

namespace Pipelines;

public static class Extension
{
    public static IAddInputBuilder AddPipeline(this IServiceCollection service)
    {
        return new PipelineBuilder(service);
    }
}