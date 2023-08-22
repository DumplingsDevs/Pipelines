using Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;
using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;
using Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

namespace Pipelines.Tests.UseCases.CrossValidationReturnType;

public class Tests
{
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherStringResult_ThrowsParameterCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<ParameterCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerStringResult<>), assembly)
                .AddDispatcher<IDispatcherStringResult>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherTaskGenericResult_ThrowsReturnTypesShouldHaveClassConstraintException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskGenericResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskGenericResult>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithConstrainedResult_ThrowsParameterCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithConstrainedResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskWithConstrainedResult>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithTwoConstrainedResults_ThrowsParameterCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithTwoConstraintedResults<,,>), assembly)
                .AddDispatcher<IDispatcherTaskWithTwoConstrainedResults>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMismatchingResultTypeCount_ThrowsResultTypeCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<ResultTypeCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskStringResult<>), assembly)
                .AddDispatcher<IDispatcherTaskWithTwoConstrainedResults>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMismatchingResultType_ThrowsTaskReturnTypeMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<TaskReturnTypeMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerStringResult<>), assembly)
                .AddDispatcher<IDispatcherTaskStringResult>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithVoidDispatcherAndHandlerWithResult_ThrowsVoidAndValueMethodMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<VoidAndValueMethodMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerStringResult<>), assembly)
                .AddDispatcher<IDispatcherVoid>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithGenericTypeCountMismatch_ThrowsGenericTypeCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<GenericTypeCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithTwoConstraintedResults<,,>), assembly)
                .AddDispatcher<IDispatcherTaskWithClassConstraintedResults>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMismatchingGenericType_ThrowsGenericTypeCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();

        var assembly = typeof(DependencyContainer).Assembly;

        Assert.Throws<GenericTypeMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithTwoConstraintedResults<,,>), assembly)
                .AddDispatcher<IDispatcherTaskWithDifferentTwoConstraintedResults>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
}