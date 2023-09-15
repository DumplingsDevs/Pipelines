using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Sample;
using Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces;

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
                new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly)
            .WithDecorator(typeof(LoggingDecorator<,>)));
        
        _dependencyContainer.BuildContainer();
        _dispatcher = _dependencyContainer.GetService<IDispatcher>();
    }

    [Test]
    public async Task HappyPathForExampleInput()
    {
        //Arrange
        var request = new ExampleInput("My test request");

        //Act
        var result = await _dispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request"));
    }

    [Test]
    public async Task HappyPathForExampleCommand2()
    {
        //Arrange
        var request = new ExampleCommand2("My test request 2");

        //Act
        var result = await _dispatcher.SendAsync(request, new CancellationToken());

        //Assert
        Assert.That(result.Value, Is.EqualTo("My test request 2"));
    }
}