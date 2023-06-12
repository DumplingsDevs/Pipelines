using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private Type _decoratorType = null!;

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
        //Dispatcher, Handler and Decorator implements method with same input / output parameters
        //Dispatcher, Handler and Decorator have same input type as provided in AddInput method
        //InputType shouldn't be object type
        //Only one method handle should be implemented in Dispatcher, Handler and Decorator
    }
}