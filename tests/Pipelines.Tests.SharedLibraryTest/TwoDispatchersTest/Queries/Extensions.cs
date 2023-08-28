using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services,
        Assembly assembly)
    {
        var sharedLibraryAssembly = typeof(Extensions).Assembly;

        services
            .AddPipeline()
            .AddInput(typeof(IQuery<>))
            .AddHandler(typeof(IQueryHandler<,>), assembly)
            .AddDispatcher<IQueryDispatcher>(new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), sharedLibraryAssembly)
            .Build();

        return services;
    }
}
