
using Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions;
using Pipelines.Tests.UseCases.InputValidation.Types;

namespace Pipelines.Tests.UseCases.InputValidation;

public class Tests
{
    [Test]
    public void Validate_WithWrongInputPosition_ThrowsDispatcherMethodInputTypeMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<DispatcherMethodInputTypeMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithWrongInputTypePosition<>), assembly)
                .AddDispatcher<IDispatcherWithWrongInputTypePosition>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
}