using Pipelines.CleanArchitecture.Abstractions.DomainEvents;

namespace Pipelines.CleanArchitecture.Domain.Events;

public class ToDoCreated : IDomainEvent
{
    public ToDoCreated(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public Guid Id { get; }
    public string Title { get; }
}