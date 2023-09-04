using Pipelines.CleanArchitecture.Abstractions.DomainEvents;

namespace Pipelines.CleanArchitecture.Infrastructure.DomainEvents;

internal class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public DomainEventsDispatcher(IDomainEventDispatcher domainEventDispatcher)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task SendAsync(IReadOnlyList<IDomainEvent> domainEvents, CancellationToken token)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _domainEventDispatcher.SendAsync(domainEvent, token);
        }
    }
}