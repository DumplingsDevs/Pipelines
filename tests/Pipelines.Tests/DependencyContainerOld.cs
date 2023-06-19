using Microsoft.Extensions.DependencyInjection;
using Pipelines.Tests.TestData;

namespace Pipelines.Tests;

public class DependencyContainerOld
{
    private IServiceProvider _provider;
    private readonly IServiceCollection _services;

    public DependencyContainerOld()
    {
        _services = new ServiceCollection();
        _services.AddScoped<ICommandHandlerWithResult<ExampleCommandWithResult>, ExampleCommandHandlerWithResult>();
        _services
            .AddPipeline()
            .AddInput(typeof(ICommandWithResult))
            .AddHandler(typeof(ICommandHandlerWithResult<>), typeof(DependencyContainerOld).Assembly)
            .AddDispatcher<ICommandDispatcher>()
            .Build();
    }

    public void BuildContainer()
    {
        _provider = _services.BuildServiceProvider();
    }

    public void RegisterMockType<TType>(object instance) where TType : class
    {
        _services.AddScoped<TType>(x => (TType)instance);
    }

    public TService GetService<TService>() where TService : notnull
    {
        return _provider.GetRequiredService<TService>();
    }

    public ICommandDispatcher GetCommandDispatcher()
    {
        return _provider.GetRequiredService<ICommandDispatcher>();
    }
}