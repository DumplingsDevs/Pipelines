using Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Sample;
using Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Types;

namespace Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly DependencyContainer _dependencyContainer;
    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;
        
        _dependencyContainer.RegisterPipeline<ICommandDispatcher>(assembly,typeof(ICommand<>), typeof(ICommandHandler<,>));
        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
    }
    
    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act
        var result = await _commandDispatcher.SendAsync(request, new CancellationToken());
            
        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request"));
    }
}