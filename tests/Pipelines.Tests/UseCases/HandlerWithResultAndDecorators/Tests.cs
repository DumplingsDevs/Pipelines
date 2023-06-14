using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;
using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators;

public class Tests
{
    private readonly IDecoratorCommandDispatcher _decoratorCommandDispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline<IDecoratorCommandDispatcher>(assembly, typeof(IDecoratorCommand<>),
            typeof(IDecoratorCommandHandler<,>));
        _dependencyContainer.BuildContainer();
        _decoratorCommandDispatcher = _dependencyContainer.GetDispatcher<IDecoratorCommandDispatcher>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new DecoratorExampleDecoratorCommand("My test request");

        //Act
        var result = await _decoratorCommandDispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request Changed"));
    }
}