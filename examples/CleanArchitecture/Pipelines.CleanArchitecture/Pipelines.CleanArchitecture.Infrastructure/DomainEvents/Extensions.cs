using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.DomainEvents;
using Pipelines.CleanArchitecture.Application;

namespace Pipelines.CleanArchitecture.Infrastructure.DomainEvents;

public static class Extensions
{
    public static void AddDomainEvents(this IServiceCollection services)
    {
        var infrastructureAssembly = typeof(Extensions).Assembly;
        var commandsAssembly = typeof(ApplicationMarker).Assembly;

        services.AddPipeline()
            .AddInput(typeof(IDomainEvent))
            .AddHandler(typeof(IDomainEventHandler<>), commandsAssembly)
            .AddDispatcher<IDomainEventDispatcher>(infrastructureAssembly);
    }
}