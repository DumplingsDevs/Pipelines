using Pipelines.Tests.TestData;

namespace Pipelines.Tests;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly DependencyContainerOld _dependencyContainerOld;
    public Tests()
    {
        _dependencyContainerOld = new DependencyContainerOld();
        _dependencyContainerOld.BuildContainer();
        _commandDispatcher = _dependencyContainerOld.GetCommandDispatcher();

    }
    
    [Test]
    public async Task Test1()
    {
        var request = new ExampleCommandWithResult("My test request", 128);

        var result = await _commandDispatcher.SendAsync(request);
            
        Assert.That(result, Is.EqualTo("It's working!, My test request, 128"));
    }
}