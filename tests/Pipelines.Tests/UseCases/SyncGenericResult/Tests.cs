using Pipelines.Exceptions;

namespace Pipelines.Tests.UseCases.SyncGenericResult;
using Types;
using Sample;

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
            .WithDecorator(typeof(LoggingDecorator<,>)));
        
        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcher>();
    }

    [Test]
    public void HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result = _dispatcher.Send(request);

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request"));
    }

    [Test]
    public void HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.Throws<HandlerNotRegisteredException>(() =>
            _dispatcher.Send(request));
    }
}