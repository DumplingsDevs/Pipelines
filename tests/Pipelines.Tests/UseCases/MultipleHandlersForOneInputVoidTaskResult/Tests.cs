using Pipelines.Exceptions;

namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult;

using Types;
using Sample;

public class Tests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(ICommand))
            .AddHandler(typeof(ICommandHandler<>), assembly)
            .AddDispatcher<ICommandDispatcher>(
                new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly)
            .WithDecorator(typeof(LoggingDecorator<>)));

        _dependencyContainer.RegisterSingleton<DecoratorsState>();

        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
        _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act
        var result = _commandDispatcher.SendAsync(request, new CancellationToken());
        await result;
        result.Wait();

        //Assert
        Assert.That(result.Status, Is.EqualTo(TaskStatus.RanToCompletion));
        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<>).Name,
            nameof(ExampleCommandHandler),
            typeof(LoggingDecorator<>).Name,
            typeof(LoggingDecorator<>).Name,
            nameof(ExampleCommandHandlerTwo),
            typeof(LoggingDecorator<>).Name,
        }, _state.Status);
    }
    
    [Test]
    public Task HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await _commandDispatcher.SendAsync(request, new CancellationToken()));

        return Task.CompletedTask;
    }
}