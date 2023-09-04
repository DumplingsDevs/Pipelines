namespace Pipelines.CleanArchitecture.Abstractions.DomainEvents;

public interface IDomainEventsDispatcher
{
    //Support for multiple inputs in the dispatcher in Pipelines will appear in the next versions of the library. At this point, this implementation must be written by application Developer.
    public Task SendAsync(IReadOnlyList<IDomainEvent> domainEvents, CancellationToken token);
}