using Pipelines.CleanArchitecture.Application.Commands.CreateToDo;
using Shouldly;

namespace Pipelines.CleanArchitecture.Application.Tests.Commands.CreateToDo;

public class CreateToDoHandlerTests : TestFixture
{
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