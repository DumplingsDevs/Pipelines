using Microsoft.Extensions.DependencyInjection;
using Pipelines.Utils;

namespace Pipelines.Builder.Decorators;

internal static class Extensions
{
    
//              ▲   ┌─────────────────────┐   │
//              │   │ Decorator1:IHandler │   │
//              │   └─────────────────────┘   │
//              │                             │
//              │   ┌─────────────────────┐   │
//              │   │ Decorator2:IHandler │   │
//              │   └─────────────────────┘   │
// Registration │                             │  Invoke
//  direction   │   ┌─────────────────────┐   │ direction
//              │   │ Decorator3:IHandler │   │
//              │   └─────────────────────┘   │
//              │                             │
//              │    ┌──────────────────┐     │
//              │    │ Handler:IHandler │     │
//              │    └──────────────────┘     ▼
// Decorators are registers and combined like matryoshka - all implements the same interface, and invokes the next one,
// until handler(the last in chain) will return result.
    internal static IServiceCollection AddHandlersWithDecorators(this IServiceCollection serviceCollection,
        List<Type> decorators, IEnumerable<Type> handlers)
    {
        foreach (var handlerType in handlers)
        {
            TryAddHandlerWithDecorators(serviceCollection, decorators, handlerType);
        }

        return serviceCollection;
    }

    private static void TryAddHandlerWithDecorators(IServiceCollection serviceCollection, List<Type> decorators,
        Type handlerType)
    {
        
        // handlerType implements generic interfaces - we are sure because we validated it in pipeline builder
        var genericTypeInterfaces = GetGenericTypeInterfaces(handlerType);

        foreach (var genericTypeInterface in genericTypeInterfaces)
        {
            var compatibleDecorators = GetCompatibleDecorators(decorators, genericTypeInterface);

            if (compatibleDecorators.Count == 0)
            {
                // there is no decorator for handler - register is as implementer handler's interface.
                // TO DO - what if user wants to register handler with different lifetime e.g. Transient?
                serviceCollection.AddScoped(genericTypeInterface, handlerType);
                continue;
            }

            AddHandlerWithCompatibleDecorators(serviceCollection, handlerType, compatibleDecorators,
                genericTypeInterface);
        }
    }

    private static void AddHandlerWithCompatibleDecorators(IServiceCollection serviceCollection, Type handlerType,
        IEnumerable<Type> compatibleDecorators, Type genericTypeInterface)
    {
        if (serviceCollection.All(x => x.ServiceType != handlerType))
        {
            serviceCollection.AddScoped(handlerType);
        }

        var lastDecorator = DecorateRecursively(serviceCollection, handlerType, genericTypeInterface, new Queue<Type>(compatibleDecorators));

        serviceCollection.AddScoped(genericTypeInterface, lastDecorator);
    }

    private static List<Type> GetCompatibleDecorators(List<Type> decorators, Type @interface)
    {
        return decorators.Where(decoratorType => CanDecorate(@interface, decoratorType)).ToList();
    }

    private static IEnumerable<Type> GetGenericTypeInterfaces(Type handlerType)
    {
        return handlerType.GetInterfaces().Where(x => x.IsGenericType);
    }

    private static Func<IServiceProvider, object> DecorateRecursively(IServiceCollection serviceCollection, Type decoratedType,
        Type genericTypeInterface, Queue<Type> compatibleDecorators)
    {
        var decoratorType = compatibleDecorators.Dequeue();

        if (decoratorType.IsGenericType)
        {
            var genericArguments = decoratedType.GetInterfaces().First(type => type.AssemblyQualifiedName == genericTypeInterface.AssemblyQualifiedName).GetGenericArguments();
            var closedDecorator = decoratorType.MakeGenericType(genericArguments);

            var closedImplementationFactory = DecoratorFactory.CreateDecorator(decoratedType, closedDecorator);
            serviceCollection.AddScoped(closedDecorator, closedImplementationFactory);

            if (compatibleDecorators.Count == 0)
            {
                return closedImplementationFactory;
            }

            return DecorateRecursively(serviceCollection, closedDecorator, genericTypeInterface, compatibleDecorators);
        }

        var implementationFactory = DecoratorFactory.CreateDecorator(decoratedType, decoratorType);
        serviceCollection.AddScoped(decoratorType, implementationFactory);
        
        if (compatibleDecorators.Count == 0)
        {
            return implementationFactory;
        }

        return DecorateRecursively(serviceCollection, decoratorType, genericTypeInterface, compatibleDecorators);
    }

    private static bool CanDecorate(Type serviceType, Type decoratorType) =>
        serviceType is { IsGenericType: true, IsGenericTypeDefinition: false }
        && serviceType.HasCompatibleGenericArguments(decoratorType);
}