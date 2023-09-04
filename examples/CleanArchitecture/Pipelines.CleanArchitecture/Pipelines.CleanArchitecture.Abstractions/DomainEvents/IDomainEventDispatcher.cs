namespace Pipelines.CleanArchitecture.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    public Task SendAsync(IDomainEvent input, CancellationToken token);
}