using Pipelines.Exceptions;
using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;
using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Queries;
using Pipelines.Tests.UseCases.PipelinesDefinitionInSharedLibrary.Sample;
using Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime.Sample;

namespace Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;
        _dependencyContainer.Services
            .AddCommands(assembly)
            .AddQueries(assembly);

        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
        _queryDispatcher = _dependencyContainer.GetService<IQueryDispatcher>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var command = new ExampleCommand("My test command");
        var query = new ExampleQuery("Id");

        //Act
        var commandResult = await _commandDispatcher.SendAsync(command, new CancellationToken());
        var queryResult = await _queryDispatcher.HandleAsync(query, new CancellationToken());

        //Assert
        Assert.That(commandResult.Id, Is.EqualTo("My test command"));
        for (int i = 0; i < 10; i++)
        {
            Assert.That(queryResult[i].Value, Is.EqualTo($"Id{i+1}"));

        }
    }

    [Test]
    public Task HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommandNotRegistered("My test request");

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await _commandDispatcher.SendAsync(request, new CancellationToken()));

        return Task.CompletedTask;
    }
}