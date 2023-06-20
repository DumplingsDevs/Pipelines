using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Decorators;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IHandlerBuilder AddInput(Type type)
    {
        _inputType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type handlerType, Assembly assembly)
    {
        _handlerType = handlerType;

        //TO DO - how to filter out decorators? Maybe we can assume that something IS NOT a decorator when there is no handler in constructor?
        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, handlerType)
            .WhereConstructorDoesNotHaveParameter(handlerType);

        _serviceCollection.RegisterGenericTypesAsScoped(types);

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        _serviceCollection.AddScoped<DispatcherInterceptor>(x =>
            new DispatcherInterceptor(x, _inputType, _handlerType));
        _serviceCollection.AddScoped<TDispatcher>(x =>
        {
            var interceptor = x.GetService<DispatcherInterceptor>();
            var proxyGenerator = new ProxyGenerator();
            return proxyGenerator.CreateInterfaceProxyWithoutTarget<TDispatcher>(interceptor);
        });

        return this;
    }

    public IPipelineBuildBuilder AddDecorators(params Type[] decorators)
    {
        foreach (var decoratorType in decorators)
        {
            for (var i = _serviceCollection.Count - 1; i >= 0; i--)
            {
                var serviceDescriptor = _serviceCollection[i];

                if (serviceDescriptor.ServiceType is DecoratedType)
                {
                    continue; // Service has already been decorated.
                }

                if (!CanDecorate(serviceDescriptor.ServiceType, decoratorType))
                {
                    continue; // decorator is not compatible with ServiceType
                }

                var decoratedType = new DecoratedType(serviceDescriptor.ServiceType);
                _serviceCollection.Add(serviceDescriptor.WithServiceType(decoratedType));

                var implementationFactory = DecoratorFactory.CreateDecorator(decoratedType, decoratorType);
                _serviceCollection[i] = serviceDescriptor.WithImplementationFactory(implementationFactory);
            }
        }

        return this;
    }

    private static bool CanDecorate(Type serviceType, Type decoratorType) =>
        serviceType is { IsGenericType: true, IsGenericTypeDefinition: false }
        && serviceType.HasCompatibleGenericArguments(decoratorType);

    public void Build()
    {
        //Dispatcher, Handler and Decorator implements method with same input / output parameters
        //Dispatcher, Handler and Decorator have same input type as provided in AddInput method
        //InputType shouldn't be object type
        //Only one method handle should be implemented in Dispatcher, Handler and Decorator
    }
}