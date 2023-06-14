using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Validators;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private Type _decoratorType = null!;
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

    public IDispatcherBuilder AddHandler(Type type, Assembly assembly)
    {
        _handlerType = type;
        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, type);
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

    public IPipelineBuildBuilder AddDecorators(Type decoratorGenericType, params Type[] decorators)
    {
        _decoratorType = decoratorGenericType;

        foreach (var decorator in decorators)
        {
            _serviceCollection.AddScoped(_decoratorType, decorator);
        }

        return this;
    }
    
    public void Build()
    {
        InputTypeShouldBeClassOrRecord.Validate(_inputType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType, _dispatcherType);
        ShouldImplementExactlySameHandleMethods.Validate(_handlerType, _dispatcherType);
    }
}