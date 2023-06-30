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

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private Type _dispatcherType = null!;
    private Type[] _decorators = null!;

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
        ProvidedTypeShouldBeInterface.Validate(handlerType);

        _handlerType = handlerType;

        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, handlerType)
            .WhereConstructorDoesNotHaveParameter(handlerType);

        _serviceCollection.RegisterGenericTypesAsScoped(types);

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        ProvidedTypeShouldBeInterface.Validate(typeof(TDispatcher));
        
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
        _decorators = decorators;

        return this;
    }

    public void Build()
    {
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType, _dispatcherType);
        MethodShouldHaveAtLeastOneParameter.Validate(_handlerType, _dispatcherType);
        
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputType, _handlerType);
        ValidateResultTypesWithHandlerGenericArguments.Validate(_handlerType);
        
        ValidateInputTypeWithDispatcherMethodParameters.Validate(_inputType, _dispatcherType);
        ValidateResultTypesWithDispatcherInputResultTypes.Validate(_inputType, _handlerType);

        _serviceCollection.AddDecorators(_decorators);
    }
}