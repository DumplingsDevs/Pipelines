

using Pipelines.Exceptions;
using Pipelines.Tests.SharedLibraryTest.Types;
using Pipelines.Tests.UseCases.PipelinesDefinitionInSharedLibrary.Sample;

namespace Pipelines.Tests.UseCases.PipelinesDefinitionInSharedLibrary;

public class Tests
{
    private readonly IDispatcherShared _dispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        
        var assembly = typeof(DependencyContainer).Assembly;
        SharedLibraryTest.SharedLibraryTest.RegisterPipelines(_dependencyContainer.Services, assembly);
        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcherShared>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result = await _dispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request"));
    }

    [Test]
    public Task HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await _dispatcher.SendAsync(request, new CancellationToken()));

        return Task.CompletedTask;
    }
}