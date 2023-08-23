
using Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters;

public class Tests
{
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcher_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithCancellationToken<>), assembly)
                .AddDispatcher<IDispatcherWithCancellationToken>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
}