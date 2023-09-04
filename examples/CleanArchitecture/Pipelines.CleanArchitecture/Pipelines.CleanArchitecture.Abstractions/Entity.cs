using Pipelines.CleanArchitecture.Abstractions.DomainEvents;

namespace Pipelines.CleanArchitecture.Abstractions;

public abstract class Entity
{
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    private List<IDomainEvent> _domainEvents;

    protected Entity()
    {
        _domainEvents = new List<IDomainEvent>();
    }

    public void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
}