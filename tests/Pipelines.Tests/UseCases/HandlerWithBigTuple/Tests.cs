using System.Drawing;
using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithBigTuple.Sample;
using Pipelines.Tests.UseCases.HandlerWithBigTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithBigTuple;

public class Tests
{
    private readonly IDispatcher _dispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<,,,,,,>))
            .AddHandler(typeof(IHandler<,,,,,,,>), assembly)
            .AddDispatcher<IDispatcher>(
                new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
            .WithDecorator(typeof(LoggingDecorator<,,,,,,,>)));

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

        //Act
        var result = _dispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Item1.Value, Is.EqualTo("My test request"));
        Assert.That(result.Item2.Value, Is.EqualTo("Value"));
        Assert.That(result.Item3.Value, Is.EqualTo(1));
        Assert.That(result.Item4.Value, Is.EqualTo(21.37));
        Assert.That(result.Item5.Value, Is.EqualTo(new Rectangle(10, 10, 10, 10)));
        Assert.That(result.Item6.Value, Is.EqualTo(DateTime.Parse("2023-05-06 22:30:00.531")));
        Assert.That(result.Item7.Value, Is.EqualTo("Value2"));

        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<,,,,,,,>).Name,
            typeof(LoggingDecorator<,,,,,,,>).Name,
        }, _state.Status);
    }

    [Test]
    public Task HandlerNotFound()
    {
        //Arrange
        var request = new ExampleCommand2("My test request");

        //Act & Assert
        Assert.Throws<HandlerNotRegisteredException>(() =>
            _dispatcher.SendAsync(request, new CancellationToken()));

        return Task.CompletedTask;
    }
}