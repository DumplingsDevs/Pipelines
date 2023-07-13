using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Utils;

namespace Pipelines.Builder.Decorators;

internal static class Extensions
{
    internal static IServiceCollection AddDecorators(this IServiceCollection serviceCollection,
        IEnumerable<Type> decorators, IEnumerable<Type> handlers)
    {
        foreach (var handlerType in handlers)
        {
            var interfaces = handlerType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType)
                {
                    var compatibleDecorators =
                        decorators.Where(decoratorType => CanDecorate(@interface, decoratorType)).ToList();

                    if (compatibleDecorators.Count == 0)
                    {
                        serviceCollection.AddScoped(@interface, handlerType);
                        continue;
                    }

                    serviceCollection.AddScoped(handlerType);
                    var lastDecorator = Decorate(serviceCollection, handlerType, new Queue<Type>(compatibleDecorators));

                    if (lastDecorator != null)
                    {
                        serviceCollection.AddScoped(@interface, lastDecorator);
                    }
                }
            }
        }

        return serviceCollection;
    }

    private static Func<IServiceProvider, object>? Decorate(IServiceCollection serviceCollection, Type decoratedType,
        Queue<Type> compatibleDecorators)
    {
        if (compatibleDecorators.Count == 0)
        {
            return null;
        }

        var decoratorType = compatibleDecorators.Dequeue();


        if (decoratorType.IsGenericType)
        {
            //TO DO: Find interface of handler type
            var genericArguments = decoratedType.GetInterfaces().First().GetGenericArguments();
            var closedDecorator = decoratorType.MakeGenericType(genericArguments);

            var closedImplementationFactory = DecoratorFactory.CreateDecorator(decoratedType, closedDecorator);

            serviceCollection.AddScoped(closedDecorator, closedImplementationFactory);
            if (compatibleDecorators.Count == 0)
            {
                return closedImplementationFactory;
            }

            return Decorate(serviceCollection, closedDecorator, compatibleDecorators);
        }

        var implementationFactory = DecoratorFactory.CreateDecorator(decoratedType, decoratorType);
        serviceCollection.AddScoped(decoratorType, implementationFactory);
        if (compatibleDecorators.Count == 0)
        {
            return implementationFactory;
        }

        return Decorate(serviceCollection, decoratorType, compatibleDecorators);
    }

    private static bool CanDecorate(Type serviceType, Type decoratorType) =>
        serviceType is { IsGenericType: true, IsGenericTypeDefinition: false }
        && serviceType.HasCompatibleGenericArguments(decoratorType);
}