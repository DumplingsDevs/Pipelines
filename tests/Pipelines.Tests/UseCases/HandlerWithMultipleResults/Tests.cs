using Pipelines.Tests.UseCases.HandlerWithMultipleResults.Sample;
using Pipelines.Tests.UseCases.HandlerWithMultipleResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly DependencyContainer _dependencyContainer;
    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;
        
        _dependencyContainer.RegisterPipeline<ICommandDispatcher>(assembly,typeof(ICommand<,>), typeof(ICommandHandler<,,>));
        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
    }
    
    //[Test]  //After we will remove limit to max one result, this test should be uncommented
    public void HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act
        var result = _commandDispatcher.SendAsync(request, new CancellationToken());
            
        //Assert
        Assert.That(result.Item1.Value, Is.EqualTo("My test request"));
        Assert.That(result.Item2, Is.EqualTo(5));
    }
}