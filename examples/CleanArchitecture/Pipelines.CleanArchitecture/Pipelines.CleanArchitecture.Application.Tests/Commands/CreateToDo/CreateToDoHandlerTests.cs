using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Application.Commands.CreateToDo;
using Pipelines.CleanArchitecture.Domain.Repositories;
using Shouldly;

namespace Pipelines.CleanArchitecture.Application.Tests.Commands.CreateToDo;

public class CreateToDoHandlerTests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IToDoRepository _toDoRepository;
    
    public CreateToDoHandlerTests()
    {
        var dependencyContainer = new DependencyContainer();
        dependencyContainer.BuildContainerAndSetupDatabase();
        _commandDispatcher = dependencyContainer.GetCommandDispatcher();
        _toDoRepository = dependencyContainer.GetService<IToDoRepository>();
    }
    
    [Test]
    public async Task CorrectCommandParameters_ToDoCreated()
    {
        //Arrange
        var command = new CreateToDoCommand("Fancy to Do");
        
        //Act
        var result = await _commandDispatcher.SendAsync(command, CancellationToken.None);
        
        //Assert
        var toDo = await _toDoRepository.GetAsync(result, CancellationToken.None);
        
        toDo.ShouldNotBeNull();
    }
}