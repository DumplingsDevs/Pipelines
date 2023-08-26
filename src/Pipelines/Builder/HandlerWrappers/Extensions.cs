using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Builder.HandlerWrappers;

internal static class Extensions
{
    internal static IServiceCollection AddHandlersRepository(this IServiceCollection serviceCollection,
        List<Type> handlers)
    {
        serviceCollection.AddSingleton<IHandlersRepository>(x => new HandlersRepository(handlers));

        return serviceCollection;
    }
}