using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Sample;
using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults;

public class Tests
{
    private readonly IDispatcher _dispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    // Test is disabled - TBD how we want handle that scenario
    public Tests()
    {
        // _dependencyContainer = new DependencyContainer();
        // var assembly = typeof(DependencyContainer).Assembly;
        //
        // _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<,>))
        //     .AddHandler(typeof(IHandler<,,,>), assembly)
        //     .AddDispatcher<IDispatcher>(
        //         new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
        //     .WithOpenTypeDecorator(typeof(LoggingDecorator<,,,>)));
        //
        // _dependencyContainer.RegisterSingleton<DecoratorsState>();
        //
        // _dependencyContainer.BuildContainer();
        // _dispatcher = _dependencyContainer.GetService<IDispatcher>();
        // _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    // [Test]
    public void HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result =
            _dispatcher.SendAsync<ExampleRecordCommandResult, ExampleCommandClassResult, ExampleCommandClassResult2>(
                request, new CancellationToken());

        //Assert
        Assert.That(result.Item1.Value, Is.EqualTo("My test request"));
        Assert.That(result.Item2.Value, Is.EqualTo("Value"));

        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<,,,>).Name,
            typeof(LoggingDecorator<,,,>).Name,
        }, _state.Status);
    }

    // [Test]
    public void HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.Throws<HandlerNotRegisteredException>(() =>
            _dispatcher.SendAsync<ExampleRecordCommandResult, ExampleCommandClassResult, ExampleCommandClassResult2>(request, new CancellationToken()));
    }
}