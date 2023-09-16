using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Abstractions.Queries;
using Pipelines.CleanArchitecture.Application.Commands.CreateToDo;

namespace Pipelines.CleanArchitecture.Infrastructure.Tests.Queries.Handlers;

public abstract class TestFixture
{
    protected readonly ICommandDispatcher _commandDispatcher;
    protected readonly IQueryDispatcher _queryDispatcher;

    protected TestFixture()
    {
        var dependencyContainer = new DependencyContainer();
        dependencyContainer.BuildContainerAndSetupDatabase();
        _commandDispatcher = dependencyContainer.GetCommandDispatcher();
        _queryDispatcher = dependencyContainer.GetQueryDispatcher();
    }

    protected async Task<Guid> CreateToDoInDatabase()
    {
        var command = new CreateToDoCommand("Fancy to Do");
        var toDoId = await _commandDispatcher.SendAsync(command, CancellationToken.None);
        return toDoId;
    }
}