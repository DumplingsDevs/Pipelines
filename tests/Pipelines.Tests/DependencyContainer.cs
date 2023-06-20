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

    public void RegisterPipeline<TDispatcher>(Assembly handlersAssembly,
        Type inputType, Type handlerType, Type[]? decorators = null) where TDispatcher : class
    {
        _services
            .AddPipeline()
            .AddInput(inputType)
            .AddHandler(handlerType, handlersAssembly)
            .AddDispatcher<TDispatcher>()
            .AddDecorators(decorators ?? Array.Empty<Type>())
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