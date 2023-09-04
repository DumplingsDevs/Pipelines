using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.DomainEvents;
using Pipelines.CleanArchitecture.Application;
using Pipelines.CleanArchitecture.Domain;

namespace Pipelines.CleanArchitecture.Infrastructure.DomainEvents;

public static class Extensions
{
    public static void AddDomainEvents(this IServiceCollection services)
    {
        var infrastructureAssembly = typeof(Extensions).Assembly;
        var domainHandlersAssembly = typeof(ApplicationMarker).Assembly;

        services.AddPipeline()
            .AddInput(typeof(IDomainEvent))
            .AddHandler(typeof(IDomainEventHandler<>), domainHandlersAssembly)
            .AddDispatcher<IDomainEventDispatcher>(infrastructureAssembly)
            .Build();

        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
    }
}