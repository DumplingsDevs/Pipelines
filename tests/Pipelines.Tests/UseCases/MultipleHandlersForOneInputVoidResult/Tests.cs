using Pipelines.Exceptions;

namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult;
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
                new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
            .WithDecorator(typeof(LoggingDecorator<>)));

        _dependencyContainer.RegisterSingleton<DecoratorsState>();

        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
        _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    [Test]
    public void HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act
        _commandDispatcher.SendAsync(request, new CancellationToken());

        //Assert
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
    public void HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.Throws<HandlerNotRegisteredException>(() =>
            _commandDispatcher.SendAsync(request, new CancellationToken()));
    }
}