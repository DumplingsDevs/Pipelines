using Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Sample;
using Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Types;

namespace Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface;

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
            .AddDispatcher<ICommandDispatcher>()
            .WithOpenTypeDecorator(typeof(LoggingDecorator<>)));

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
            typeof(LoggingDecorator<>).Name,
        }, _state.Status);
    }
}