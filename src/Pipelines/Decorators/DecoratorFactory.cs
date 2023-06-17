using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Decorators;

public static class DecoratorFactory
{
    internal static Func<IServiceProvider, object> CreateDecorator(Type serviceType, Type decoratorType)
    {

        var genericArguments = serviceType.GetGenericArguments();
        var closedDecorator = decoratorType.MakeGenericType(genericArguments);

        return TypeDecorator(serviceType, closedDecorator);

    }
    
    private static Func<IServiceProvider, object> TypeDecorator(Type serviceType, Type decoratorType) => serviceProvider =>
    {
        var instanceToDecorate = serviceProvider.GetRequiredService(serviceType);
        return ActivatorUtilities.CreateInstance(serviceProvider, decoratorType, instanceToDecorate);
    };
}