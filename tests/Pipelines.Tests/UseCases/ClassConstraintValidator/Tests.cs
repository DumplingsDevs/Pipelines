using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;
using Pipelines.Tests.UseCases.ClassConstraintValidator.Sample;
using Pipelines.Tests.UseCases.ClassConstraintValidator.Types;

namespace Pipelines.Tests.UseCases.ClassConstraintValidator;

public class Tests
{

    [Test]
    public void ItShouldThrowExceptionThatClassConstraintIsMissing()
    {
        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>( () =>
        {
            var dependencyContainer = new DependencyContainer();

            var assembly = typeof(DependencyContainer).Assembly;

            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<>))
                .AddHandler(typeof(IHandler<,>), assembly)
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));

            dependencyContainer.BuildContainer();
        });
    }
}