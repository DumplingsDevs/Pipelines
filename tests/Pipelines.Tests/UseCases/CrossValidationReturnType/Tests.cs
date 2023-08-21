using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

namespace Pipelines.Tests.UseCases.CrossValidationReturnType;

public class Tests
{
    [Test]
    public void HappyPath()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<TaskReturnTypeMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInput<>))
                .AddHandler(typeof(IHandler<,>), assembly)
                .AddDispatcher<IDispatcher>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
}