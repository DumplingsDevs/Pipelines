using Microsoft.Extensions.DependencyInjection;
using Pipelines.Utils;

namespace Pipelines.Builder.Decorators;

internal static class Extensions
{
    internal static IServiceCollection AddDecorators(this IServiceCollection serviceCollection, IEnumerable<Type> decorators)
    {
        foreach (var decoratorType in decorators)
        {
            AddDecorator(serviceCollection, decoratorType);
        }

        return serviceCollection;
    }

    private static void AddDecorator(IServiceCollection serviceCollection, Type decoratorType)
    {
        for (var i = serviceCollection.Count - 1; i >= 0; i--)
        {
            var serviceDescriptor = serviceCollection[i];

            if (serviceDescriptor.ServiceType is DecoratedType)
            {
                continue; // Service has already been decorated.
            }

            if (!CanDecorate(serviceDescriptor.ServiceType, decoratorType))
            {
                continue; // decorator is not compatible with ServiceType
            }

            var decoratedType = new DecoratedType(serviceDescriptor.ServiceType);
            serviceCollection.Add(serviceDescriptor.WithServiceType(decoratedType));

            var implementationFactory = DecoratorFactory.CreateDecorator(decoratedType, decoratorType);
            serviceCollection[i] = serviceDescriptor.WithImplementationFactory(implementationFactory);
        }
    }

    private static bool CanDecorate(Type serviceType, Type decoratorType) =>
        serviceType is { IsGenericType: true, IsGenericTypeDefinition: false }
        && serviceType.HasCompatibleGenericArguments(decoratorType);
}