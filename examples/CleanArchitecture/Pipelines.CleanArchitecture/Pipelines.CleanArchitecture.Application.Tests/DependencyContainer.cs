using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Abstractions.Queries;
using Pipelines.CleanArchitecture.Infrastructure.Commands;
using Pipelines.CleanArchitecture.Infrastructure.DomainEvents;
using Pipelines.CleanArchitecture.Infrastructure.Persistance;

namespace Pipelines.CleanArchitecture.Application.Tests;

public class DependencyContainer
{
    private IServiceProvider _provider;
    private readonly IServiceCollection _services;
    
    public DependencyContainer()
    {
        _services = new ServiceCollection();
        _services.AddCommands();
        _services.AddDomainEvents();
        _services.AddPersistence();
    }

    public void BuildContainerAndSetupDatabase()
    {
        _provider = _services.BuildServiceProvider();
        
        DatabaseCreator.CreateDatabaseSchema(_provider);
    }

    public void RegisterMockType<TType>(object instance) where TType: class
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