using System.Reflection;
using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators;
using Types;
using Sample;

public class Tests
{
    private readonly IRequestDispatcher _requestDispatcher;
    private readonly DependencyContainer _dependencyContainer;
    private readonly DecoratorsState _state;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;

        _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IRequest<>))
            .AddHandler(typeof(IRequestHandler<,>), assembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation},
                assembly)
            .WithDecorator(typeof(LoggingDecorator<,>))
            .WithDecorator(typeof(TracingDecorator<,>))
            .WithDecorators(x =>
            {
                x.WithImplementedInterface<IDecorator>();
                x.WithInheritedClass<BaseDecorator>();
                x.WithAttribute<DecoratorAttribute>().OrderBy(attr => attr.Index);
                x.WithNameContaining("ExampleRequestDecoratorFourUniqueNameForSearch");
            }, Assembly.GetExecutingAssembly()));

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
        CollectionAssert.AreEqual(new List<string>
        {
            typeof(LoggingDecorator<,>).Name,
            typeof(TracingDecorator<,>).Name,
            nameof(ExampleRequestDecoratorFive),
            nameof(ExampleRequestDecoratorOne),
            nameof(ExampleRequestDecoratorTwo),
            nameof(ExampleRequestDecoratorThreeTwo),
            nameof(ExampleRequestDecoratorThreeOne),
            nameof(ExampleRequestDecoratorFourUniqueNameForSearch),
            nameof(ExampleRequestDecoratorFourUniqueNameForSearch),
            nameof(ExampleRequestDecoratorThreeOne),
            nameof(ExampleRequestDecoratorThreeTwo),
            nameof(ExampleRequestDecoratorTwo),
            nameof(ExampleRequestDecoratorOne),
            nameof(ExampleRequestDecoratorFive),
            typeof(TracingDecorator<,>).Name,
            typeof(LoggingDecorator<,>).Name,
        }, _state.Status);
    }
    
    [Test]
    public Task HandlerNotFound()
    {
        //Arrange
        var request = new ExampleRequest2("My test request");

        //Act & Assert
        Assert.ThrowsAsync<HandlerNotRegisteredException>(async () =>
            await _requestDispatcher.SendAsync(request, new CancellationToken()));

        return Task.CompletedTask;
    }
}