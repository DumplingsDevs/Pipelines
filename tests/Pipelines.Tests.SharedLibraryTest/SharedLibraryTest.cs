using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Tests.SharedLibraryTest.Types;

namespace Pipelines.Tests.SharedLibraryTest;

public class SharedLibraryTest
{
    public static void RegisterPipelines(IServiceCollection services, Assembly handlerAssembly)
    {
        services.AddPipeline()
            .AddInput(typeof(IInputShared<>))
            .AddHandler(typeof(IHandlerShared<,>), handlerAssembly)
            .AddDispatcher<IDispatcherShared>(new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation),
                typeof(SharedLibraryTest).Assembly)
            .WithDecorator(typeof(LoggingDecorator<,>))
            .Build();
    }
}