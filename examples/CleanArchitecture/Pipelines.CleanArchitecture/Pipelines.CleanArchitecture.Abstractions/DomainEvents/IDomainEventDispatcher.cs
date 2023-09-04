namespace Pipelines.CleanArchitecture.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    public Task SendAsync(IDomainEvent domainEvent, CancellationToken token);
}