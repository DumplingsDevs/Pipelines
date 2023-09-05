namespace Pipelines.CleanArchitecture.Abstractions.DomainEvents;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    public Task HandleAsync(TDomainEvent @event, CancellationToken token);
}