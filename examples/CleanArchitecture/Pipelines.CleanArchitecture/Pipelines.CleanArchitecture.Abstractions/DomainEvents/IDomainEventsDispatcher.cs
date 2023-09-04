namespace Pipelines.CleanArchitecture.Abstractions.DomainEvents;

public interface IDomainEventsDispatcher
{
    public Task SendAsync(IReadOnlyList<IDomainEvent> domainEvents, CancellationToken token);
}