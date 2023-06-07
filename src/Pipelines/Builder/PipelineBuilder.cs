using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IAddInputBuilder, IAddHandlerBuilder, IAddDispatcherBuilder, IAddPipelineBehaviorsBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IAddHandlerBuilder AddInput(Type type)
    {
        _inputType = type;
        return this;
    }

    public IAddDispatcherBuilder AddHandler(Type type, Assembly assembly)
    {
        _handlerType = type;
        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, type);
        _serviceCollection.RegisterGenericTypesAsScoped(types);
        
        return this;
    }

    public IAddPipelineBehaviorsBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        _serviceCollection.AddScoped<DispatcherInterceptor>(x => new DispatcherInterceptor(x, _handlerType));
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
        //To do validation!
    }
}