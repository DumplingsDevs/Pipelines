using Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;
using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
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
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherTaskGenericResult_ThrowsGenericTypeCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskGenericResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskGenericResult>(
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithConstrainedResult_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithConstrainedResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskWithConstrainedResult>(
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherAndGenericInputResult_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputGenericType<>))
                .AddHandler(typeof(IHandlerTaskInputGenericResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskInputGenericResult>(
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithTwoConstrainedResults_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithTwoConstraintedResults<,,>), assembly)
                .AddDispatcher<IDispatcherTaskWithTwoConstrainedResults>(
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
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
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
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
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
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
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
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
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
    
    [Test]
    public void Validate_WithStringAndGenericType_ThrowsGenericTypeCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.Throws<IsGenericMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithConstrainedResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskStringResult2>(
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
    
    [Test]
    public void Validate_WithMismatchTuple_ThrowsGenericTypeCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.Throws<ResultTypeCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerTaskWithConstrainedResult<,>), assembly)
                .AddDispatcher<IDispatcherTaskWithMultipleResults>(
                    new DispatcherOptions() {UseReflectionProxyImplementation = EnvVariables.UseReflectionProxyImplementation}, assembly));
        });
    }
}