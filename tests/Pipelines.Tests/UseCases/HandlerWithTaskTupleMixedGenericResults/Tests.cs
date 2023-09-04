using Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;
using Pipelines.Exceptions;
using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Sample;
using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults;

public class Tests
{
    [Test]
    public void HappyPath()
    {
        var dependencyContainer = new DependencyContainer();
        var assembly = typeof(DependencyContainer).Assembly;
        Assert.Throws<ResultTypeCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<,>))
                .AddHandler(typeof(IHandler<,,,>), assembly)
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly)
                .WithDecorator(typeof(LoggingDecorator<,,,>)));
        
            dependencyContainer.RegisterSingleton<DecoratorsState>();
        
            dependencyContainer.BuildContainer();
        });
    }
}