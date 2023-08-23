
using Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;
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
    
    [Test]
    public void Validate_WithMismatchingNumberOfParameters_ThrowsParameterCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.Throws<ParameterCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithCancellationToken<>), assembly)
                .AddDispatcher<IDispatcherWithThreeParameters>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMismatchingParameterTypes_ThrowsParameterTypeMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.Throws<ParameterTypeMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithThreeParameters<>), assembly)
                .AddDispatcher<IDispatcherWithThreeParametersDifferentThanHandler>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithOneParam_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithInputTypeOnly<>), assembly)
                .AddDispatcher<IDispatcherWithInputTypeOnly>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithTwoParams_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithCancellationToken<>), assembly)
                .AddDispatcher<IDispatcherWithCancellationToken2>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithThreeParams_DoesNotThrowException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.DoesNotThrow(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithThreeParameters<>), assembly)
                .AddDispatcher<IDispatcherWithThreeParameters2>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
    [Test]
    public void Validate_WithHandlerHavingMoreParameters_ThrowsParameterCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.Throws<ParameterCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithThreeParameters<>), assembly)
                .AddDispatcher<IDispatcherWithCancellationToken3>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
    
        
    [Test]
    public void Validate_WithDispatcherHavingMoreParameters_ThrowsParameterCountMismatchException()
    {
        var dependencyContainer = new DependencyContainer();
    
        var assembly = typeof(DependencyContainer).Assembly;
    
        Assert.Throws<ParameterCountMismatchException>(() =>
        {
            dependencyContainer.RegisterPipeline(builder => builder.AddInput(typeof(IInputType))
                .AddHandler(typeof(IHandlerWithCancellationToken<>), assembly)
                .AddDispatcher<IDispatcherWithThreeParameters3>(
                    new DispatcherOptions(EnvVariables.UseReflectionProxyImplementation), assembly));
        });
    }
}