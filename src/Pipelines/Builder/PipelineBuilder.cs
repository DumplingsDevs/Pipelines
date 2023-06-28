using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Decorators;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Validators;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private Type _dispatcherType = null!;
    private DecoratorsBuilder _decoratorsBuilder = new();

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

    public void Build()
    {
        AllProvidedTypesShouldBeInterface.Validate(_inputType, _handlerType, _dispatcherType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType, _dispatcherType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputType, _handlerType);
        ValidateHandlerHandleMethod.Validate(_handlerType);

        var decorators = _decoratorsBuilder.GetDecorators();
        decorators.Reverse();
        _serviceCollection.AddDecorators(decorators);
    }

    public IPipelineDecoratorBuilder WithOpenTypeDecorator(Type genericDecorator)
    {
        // Validate if contains proper generic implementation
        // Validate if decorator's constructor has parameter with generic handler type 

        _decoratorsBuilder.AddDecorator(genericDecorator);

        return this;
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorator<T>()
    {
        // Validate if contains proper generic implementation
        // Validate if decorator's constructor has parameter with generic handler type 

        _decoratorsBuilder.AddDecorator(typeof(T));

        return this;
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies)
    {
        _decoratorsBuilder.BuildDecorators(action, _handlerType, assemblies);

        return this;
    }
}