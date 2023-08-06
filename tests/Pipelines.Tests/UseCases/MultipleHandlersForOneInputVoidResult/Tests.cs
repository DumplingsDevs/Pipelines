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

        _dependencyContainer.RegisterPipeline<ICommandDispatcher>(assembly, typeof(ICommand), typeof(ICommandHandler<>),
            builder =>
            {
                builder
                    .WithOpenTypeDecorator(typeof(LoggingDecorator<>));
            });

        _dependencyContainer.RegisterSingleton<DecoratorsState>();

        _dependencyContainer.BuildContainer();
        _commandDispatcher = _dependencyContainer.GetService<ICommandDispatcher>();
        _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    //[Test]
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
            nameof(ExampleCommandHandlerTwo),
            typeof(LoggingDecorator<>).Name,
        }, _state.Status);
    }
}