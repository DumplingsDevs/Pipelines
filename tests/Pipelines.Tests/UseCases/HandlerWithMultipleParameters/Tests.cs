using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;
using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(ICommand<>))
            .AddHandler(typeof(ICommandHandler<,>), assembly)
            .AddDispatcher<ICommandDispatcher>(
                new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));

        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act
        var result =
            await _commandDispatcher.SendAsync(request, new CancellationToken(), true,
                new Dictionary<string, string>());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request"));
    }

    [Test]
    public Task HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () => await _commandDispatcher.SendAsync(request,
            new CancellationToken(), true,
            new Dictionary<string, string>()));
        
        return Task.CompletedTask;
    }
}