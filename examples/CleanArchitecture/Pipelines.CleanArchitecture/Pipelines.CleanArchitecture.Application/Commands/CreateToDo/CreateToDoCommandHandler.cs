using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Abstractions.DomainEvents;
using Pipelines.CleanArchitecture.Domain;
using Pipelines.CleanArchitecture.Domain.Repositories;

namespace Pipelines.CleanArchitecture.Application.Commands.CreateToDo;

public class CreateToDoCommandHandler : ICommandHandler<CreateToDoCommand>
{
    private IToDoRepository _toDoRepository;
    private IDomainEventsDispatcher _domainEventsDispatcher;

    public CreateToDoCommandHandler(IToDoRepository toDoRepository, IDomainEventsDispatcher domainEventsDispatcher)
    {
        _toDoRepository = toDoRepository;
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public async Task<Guid> HandleAsync(CreateToDoCommand command, CancellationToken token)
    {
        var toDo = ToDo.Create(command.Title);
        await _toDoRepository.AddAsync(toDo, token);

        await _domainEventsDispatcher.SendAsync(toDo.DomainEvents, token);

        return toDo.Id;
    }
}