using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Builder.Decorators;

internal static class DecoratorFactory
{
    internal static Func<IServiceProvider, object> CreateDecorator(Type serviceType, Type decoratorType)
    {

        if (decoratorType.IsGenericType)
        {
            var genericArguments = serviceType.GetGenericArguments();
            var closedDecorator = decoratorType.MakeGenericType(genericArguments);

            return TypeDecorator(serviceType, closedDecorator);
        }

        return TypeDecorator(serviceType, decoratorType);
    }
    
    private static Func<IServiceProvider, object> TypeDecorator(Type serviceType, Type decoratorType) => serviceProvider =>
    {
        var instanceToDecorate = serviceProvider.GetRequiredService(serviceType);
        return ActivatorUtilities.CreateInstance(serviceProvider, decoratorType, instanceToDecorate);
    };
}