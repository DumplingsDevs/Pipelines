using Pipelines.Tests.UseCases.VoidHandler.Sample;
using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler;

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

    [Test]
    public void HappyPath()
    {
        //Arrange
        var request = new ExampleCommand("My test request");

        //Act & Assert
        Assert.DoesNotThrow(() => _commandDispatcher.SendAsync(request, new CancellationToken()));
        
        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<>).Name,
            typeof(LoggingDecorator<>).Name,
        }, _state.Status);
    }
}