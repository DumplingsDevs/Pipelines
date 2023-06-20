using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;
using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators;

public class Tests
{
    private readonly IRequestDispatcher _requestDispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline<IRequestDispatcher>(assembly, typeof(IRequest<>),
            typeof(IRequestHandler<,>));
        _dependencyContainer.BuildContainer();
        _requestDispatcher = _dependencyContainer.GetDispatcher<IRequestDispatcher>();
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
    }
}