using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pipelines.Tests.UseCases.HandlerWithResult.Sample;
using Pipelines.Tests.UseCases.HandlerWithResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResult;

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
            .WithOpenTypeDecorator(typeof(LoggingDecorator<,>)));
        
        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcher>();
    }

    [Test]
    public async Task HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result = await _dispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request"));
    }
}