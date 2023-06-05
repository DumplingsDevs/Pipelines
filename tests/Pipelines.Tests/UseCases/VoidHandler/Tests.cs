using Pipelines.Tests.UseCases.VoidHandler.Sample;
using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly DependencyContainer _dependencyContainer;
    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;
        
        _dependencyContainer.RegisterPipeline<ICommandDispatcher>(nameof(ICommandDispatcher.SendAsync), assembly, typeof(ICommandHandler<>));

        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetDispatcher<ICommandDispatcher>();
    }
    
    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act
        var result = _commandDispatcher.SendAsync(request, new CancellationToken());
        result.Wait();
        
        //Assert
        Assert.That(result.Status, Is.EqualTo(TaskStatus.RanToCompletion));
    }
}