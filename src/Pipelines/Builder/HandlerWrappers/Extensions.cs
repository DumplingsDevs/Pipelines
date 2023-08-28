using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Builder.HandlerWrappers;

internal static class Extensions
{
    internal static IServiceCollection AddHandlersRepository(this IServiceCollection serviceCollection,
        Type dispatcherInterfaceType, List<Type> handlers)
    {
        serviceCollection.AddSingleton<IHandlersRepository>(x =>
            new HandlersRepository(handlers, dispatcherInterfaceType));

        return serviceCollection;
    }
}