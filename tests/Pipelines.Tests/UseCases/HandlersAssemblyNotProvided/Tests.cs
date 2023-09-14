using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithResult.Sample;

namespace Pipelines.Tests.UseCases.HandlersAssemblyNotProvided;

using Types;

public class Tests
{
    private readonly IDispatcher _dispatcher;
    private readonly DependencyContainer _dependencyContainer;

    public Tests()
    {
        _dependencyContainer = new DependencyContainer();
    }

    [Test]
    public void HandlersAssemblyNotProvided_ThrowsException()
    {
        //Arrange
        var assembly = typeof(DependencyContainer).Assembly;

        //Act & Assert
        Assert.Throws<AssemblyNotProvidedException>(() =>
        {
            _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<>))
                .AddHandler(typeof(IHandler<,>))
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }

    [Test]
    public void DecoratorsAssemblyNotProvided_ThrowsException()
    {
        //Arrange
        var assembly = typeof(DependencyContainer).Assembly;

        //Act & Assert
        Assert.Throws<AssemblyNotProvidedException>(() =>
        {
            _dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<>))
                .AddHandler(typeof(IHandler<,>), assembly)
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
                .WithDecorators(x => x.WithNameContaining("Attribute")));
        });
    }
}