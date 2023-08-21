using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.VoidHandler.Sample;
using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler;

public class Tests
{
    private readonly IDispatcher _dispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput))
            .AddHandler(typeof(IHandler<>), assembly)
            .AddDispatcher<IDispatcher>(
                new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
            .WithOpenTypeDecorator(typeof(LoggingDecorator<>)));

        _dependencyContainer.RegisterSingleton<DecoratorsState>();

        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcher>();
        _state = _dependencyContainer.GetService<DecoratorsState>();
    }

    [Test]
    public void HappyPath()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act & Assert
        Assert.DoesNotThrow(() => _dispatcher.SendAsync(request, new CancellationToken()));

        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<>).Name,
            nameof(ExampleHandler),
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
            _dispatcher.SendAsync(request, new CancellationToken()));
    }
}