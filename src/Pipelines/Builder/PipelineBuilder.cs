using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Decorators;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Validators.Dispatcher.InputType;
using Pipelines.Builder.Validators.Dispatcher.ResultTypes;
using Pipelines.Builder.Validators.Handler.InputType;
using Pipelines.Builder.Validators.Handler.ResultTypes;
using Pipelines.Builder.Validators.Shared.InterfaceConstraint;
using Pipelines.Builder.Validators.Shared.MethodWithOneParameter;
using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Assembly _handlerAssembly = null!;
    private Type _inputType = null!;
    private Type _dispatcherType = null!;
    private DecoratorsBuilder _decoratorsBuilder = new();

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IHandlerBuilder AddInput(Type type)
    {
        ProvidedTypeShouldBeInterface.Validate(type);

        _inputType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type handlerType, Assembly assembly)
    {
        _handlerType = handlerType;
        _handlerAssembly = assembly;
        ProvidedTypeShouldBeInterface.Validate(_handlerType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType);
        MethodShouldHaveAtLeastOneParameter.Validate(_handlerType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputType, _handlerType);
        ValidateResultTypesWithHandlerGenericArguments.Validate(_handlerType);

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        _dispatcherType = typeof(TDispatcher);

        ProvidedTypeShouldBeInterface.Validate(_dispatcherType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _dispatcherType);
        MethodShouldHaveAtLeastOneParameter.Validate(_dispatcherType);
        ValidateInputTypeWithDispatcherMethodParameters.Validate(_inputType, _dispatcherType);
        ValidateResultTypesWithDispatcherInputResultTypes.Validate(_inputType, _dispatcherType);

        //TO DO - Move to Build Method
        RegisterDispatcher<TDispatcher>();

        return this;
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
        // TO DO change method name
        _decoratorsBuilder.BuildDecorators(action, _handlerType, assemblies);

        return this;
    }

    public void Build()
    {
        RegisterHandlers();
        RegisterDecorators();
    }

    private void RegisterDecorators()
    {
        var decorators = _decoratorsBuilder.GetDecorators();
        decorators.Reverse();
        _serviceCollection.AddDecorators(decorators);
    }

    private void RegisterHandlers()
    {
        var types = AssemblyScanner.GetTypesBasedOnGenericType(_handlerAssembly, _handlerType)
            .WhereConstructorDoesNotHaveParameter(_handlerType);

        _serviceCollection.RegisterGenericTypesAsScoped(types);
    }
    
    private void RegisterDispatcher<TDispatcher>() where TDispatcher : class
    {
        _serviceCollection.AddScoped<DispatcherInterceptor>(x =>
            new DispatcherInterceptor(x, _inputType, _handlerType));
        _serviceCollection.AddScoped<TDispatcher>(x =>
        {
            var interceptor = x.GetService<DispatcherInterceptor>();
            var proxyGenerator = new ProxyGenerator();
            return proxyGenerator.CreateInterfaceProxyWithoutTarget<TDispatcher>(interceptor);
        });
    }
}