using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;
using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;
using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators;

public class Tests
{
    private readonly IRequestDispatcher _requestDispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline<IRequestDispatcher>(assembly, typeof(IRequest<>),
            typeof(IRequestHandler<,>),
            new[] { typeof(LoggingDecorator<,>), typeof(TracingDecorator<,>), typeof(ExampleRequestValidator) });

        _dependencyContainer.RegisterSingleton<DecoratorsState>();

        _dependencyContainer.BuildContainer();
        _requestDispatcher = _dependencyContainer.GetService<IRequestDispatcher>();
        _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleRequest("My test request");

        //Act
        var result = await _requestDispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request Changed"));
        Assert.That(_state.Status,
            Is.EquivalentTo(new List<string>
            {
                "ExampleRequestValidator",
                "TracingDecorator",
                "LoggingDecorator",
                "LoggingDecorator",
                "TracingDecorator",
                "ExampleRequestValidator"
            }));
    }
}