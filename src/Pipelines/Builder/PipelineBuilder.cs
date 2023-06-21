using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Decorators;
using Pipelines.Builder.Validators;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private Type _dispatcherType = null!;

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

        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, handlerType)
            .WhereConstructorDoesNotHaveParameter(handlerType);

        _serviceCollection.RegisterGenericTypesAsScoped(types);

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        _dispatcherType = typeof(TDispatcher);

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
        AllProvidedTypesShouldBeInterface.Validate(_inputType, _handlerType, _dispatcherType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType, _dispatcherType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputType, _handlerType);
        ValidateHandlerHandleMethod.Validate(_handlerType);
    }
}