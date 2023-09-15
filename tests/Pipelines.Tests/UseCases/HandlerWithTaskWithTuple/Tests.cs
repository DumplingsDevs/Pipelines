using Pipelines.Exceptions;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple;

using Types;
using Sample;

public class Tests
{
    private readonly IDispatcher _dispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<,>))
            .AddHandler(typeof(IHandler<,,>), assembly)
            .AddDispatcher<IDispatcher>(
                new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly)
            .WithDecorator(typeof(LoggingDecorator<,,>)));

        _dependencyContainer.RegisterSingleton<DecoratorsState>();

        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcher>();
        _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result = await _dispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Item1.Value, Is.EqualTo("My test request"));
        Assert.That(result.Item2.Value, Is.EqualTo("Value"));

        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<,,>).Name,
            typeof(LoggingDecorator<,,>).Name,
        }, _state.Status);
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