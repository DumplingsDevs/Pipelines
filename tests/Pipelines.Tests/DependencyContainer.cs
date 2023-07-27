using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Public;

namespace Pipelines.Tests;

public class DependencyContainer
{
    private IServiceProvider _provider;
    private readonly IServiceCollection _services;

    public DependencyContainer()
    {
        _services = new ServiceCollection();
    }

    public void RegisterPipeline<TDispatcher>(Assembly handlersAssembly,
        Type inputType, Type handlerType, Action<IPipelineDecoratorBuilder>? decoratorBuilder = null) where TDispatcher : class
    {
        var builder = _services
            .AddPipeline()
            .AddInput(inputType)
            .AddHandler(handlerType, handlersAssembly)
            .AddDispatcher<TDispatcher>();

        decoratorBuilder?.Invoke(builder);

        builder.Build();
    }

    public void BuildContainer()
    {
        _provider = _services.BuildServiceProvider();
    }
    
    public void RegisterSingleton<TType>() where TType : class
    {
        _services.AddSingleton<TType>();
    }

    public void RegisterType<TInterface, TType>() where TType : class, TInterface
        where TInterface : class
    {
        _services.AddScoped<TInterface, TType>();
    }

    public TType GetService<TType>() where TType : class
    {
        return _provider.GetRequiredService<TType>();
    }
}