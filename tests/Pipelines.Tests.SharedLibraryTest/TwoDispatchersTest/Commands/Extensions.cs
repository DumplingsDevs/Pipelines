using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services, Assembly assembly)
    {
        var sharedLibraryAssembly = typeof(Extensions).Assembly;

        services
            .AddPipeline()
            .AddInput(typeof(ICommand<>))
            .AddHandler(typeof(ICommandHandler<,>), assembly)
            .AddDispatcher<ICommandDispatcher>(new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation},
                sharedLibraryAssembly)
            .Build();

        return services;
    }
}