using Pipelines.CleanArchitecture.Abstractions;
using Pipelines.CleanArchitecture.Domain.Events;

namespace Pipelines.CleanArchitecture.Domain;

public class ToDo : Entity
{
    public ToDo(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public Guid Id { get; }
    public string Title { get; }

    public static ToDo Create(string title)
    {
        var id = Guid.NewGuid();
        var todo = new ToDo(id, title);
        todo.AddDomainEvent(new ToDoCreated(id,title));
        
        return todo;
    }
}