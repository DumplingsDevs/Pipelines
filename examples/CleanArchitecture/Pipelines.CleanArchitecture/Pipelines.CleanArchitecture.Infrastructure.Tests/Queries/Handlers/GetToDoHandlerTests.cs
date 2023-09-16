using Pipelines.CleanArchitecture.Application.Queries.GetToDo;
using Shouldly;

namespace Pipelines.CleanArchitecture.Infrastructure.Tests.Queries.Handlers;

public class GetToDoHandlerTests : TestFixture
{
    [Test]
    public async Task GetToDo_ToDoExistsInDatabase_ResultShouldNotBeNull()
    {
        //Arrange
        var toDoId = await CreateToDoInDatabase();

        //Assert
        var toDo = await _queryDispatcher.SendAsync(new GetToDoQuery(toDoId), CancellationToken.None);

        //Assert
        toDo.ShouldNotBeNull();
    }
}