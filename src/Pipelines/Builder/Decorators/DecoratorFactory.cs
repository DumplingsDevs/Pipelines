using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Builder.Decorators;

internal static class DecoratorFactory
{
    internal static Func<IServiceProvider, object> CreateDecorator(Type serviceType, Type decoratorType)
    {
        return TypeDecorator(serviceType, decoratorType);
    }

    private static Func<IServiceProvider, object> TypeDecorator(Type serviceType, Type decoratorType) =>
        serviceProvider =>
        {
            var instanceToDecorate = serviceProvider.GetService(serviceType);
            return ActivatorUtilities.CreateInstance(serviceProvider, decoratorType, instanceToDecorate);
        };
}