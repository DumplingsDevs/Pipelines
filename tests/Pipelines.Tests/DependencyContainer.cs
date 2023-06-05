using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Tests;

public class DependencyContainer
{
    private IServiceProvider _provider;
    private readonly IServiceCollection _services;

    public DependencyContainer()
    {
        _services = new ServiceCollection();
    }

    public void RegisterPipeline<TDispatcher>(string nameOfDispatcherMethod, Assembly handlersAssembly,
        Type handlerType) where TDispatcher : class
    {
        _services
            .AddPipeline()
            .AddHandler(handlerType, handlersAssembly)
            .AddDispatcher<TDispatcher>(nameOfDispatcherMethod)
            .Build();
    }

    public void BuildContainer()
    {
        _provider = _services.BuildServiceProvider();
    }

    public void RegisterType<TInterface, TType>() where TType : class, TInterface
        where TInterface : class
    {
        _services.AddScoped<TInterface, TType>();
    }

    public TDispatcher GetDispatcher<TDispatcher>() where TDispatcher : class
    {
        return _provider.GetRequiredService<TDispatcher>();
    }
}