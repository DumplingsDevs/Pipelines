using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;
using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters;

public class Tests
{
    private readonly IDispatcher _dispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<>))
            .AddHandler(typeof(IHandler<,>), assembly)
            .AddDispatcher<IDispatcher>(
                new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
            .WithDecorator(typeof(LoggingDecorator<,>)));

        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcher>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result =
            await _dispatcher.SendAsync(request, new CancellationToken(), true,
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
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () => await _dispatcher.SendAsync(request,
            new CancellationToken(), true,
            new Dictionary<string, string>()));
        
        return Task.CompletedTask;
    }
}