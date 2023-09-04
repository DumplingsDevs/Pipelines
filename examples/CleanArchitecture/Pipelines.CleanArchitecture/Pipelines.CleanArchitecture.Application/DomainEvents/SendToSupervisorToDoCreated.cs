using Pipelines.CleanArchitecture.Abstractions.DomainEvents;
using Pipelines.CleanArchitecture.Domain.Events;

namespace Pipelines.CleanArchitecture.Application.DomainEvents;

public class SendToSupervisorToDoCreated: IDomainEventHandler<ToDoCreated>
{
    public Task HandleAsync(ToDoCreated @event, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}