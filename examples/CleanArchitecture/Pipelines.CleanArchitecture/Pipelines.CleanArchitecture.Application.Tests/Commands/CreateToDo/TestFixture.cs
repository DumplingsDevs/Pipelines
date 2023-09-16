using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Domain.Repositories;

namespace Pipelines.CleanArchitecture.Application.Tests.Commands.CreateToDo;

public abstract class TestFixture
{
    protected readonly ICommandDispatcher _commandDispatcher;
    protected readonly IToDoRepository _toDoRepository;

    protected TestFixture()
    {
        var dependencyContainer = new DependencyContainer();
        dependencyContainer.BuildContainerAndSetupDatabase();
        _commandDispatcher = dependencyContainer.GetCommandDispatcher();
        _toDoRepository = dependencyContainer.GetService<IToDoRepository>();
    }
}