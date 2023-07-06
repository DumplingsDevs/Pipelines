using Pipelines.Builder.Validators.CrossValidation.MethodParameters;
using Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;
using Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters;

[TestFixture]
public class CrossValidateMethodParametersTests
{
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcher_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithCancellationToken<IInputType>);
        var dispatcherType = typeof(IDispatcherWithCancellationToken);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMismatchingNumberOfParameters_ThrowsParameterCountMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithCancellationToken<IInputType>);
        var dispatcherType = typeof(IDispatcherWithThreeParameters);

        // Act & Assert
        Assert.Throws<ParameterCountMismatchException>(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMismatchingParameterTypes_ThrowsParameterTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithThreeParameters<IInputType>);
        var dispatcherType = typeof(IDispatcherWithThreeParametersDifferentThanHandler);

        // Act & Assert
        Assert.Throws<ParameterTypeMismatchException>(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithOneParam_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithInputTypeOnly<IInputType>);
        var dispatcherType = typeof(IDispatcherWithInputTypeOnly);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }
    
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithTwoParams_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithCancellationToken<IInputType>);
        var dispatcherType = typeof(IDispatcherWithCancellationToken);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithThreeParams_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithThreeParameters<IInputType>);
        var dispatcherType = typeof(IDispatcherWithThreeParameters);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithHandlerHavingMoreParameters_ThrowsParameterCountMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithThreeParameters<IInputType>);
        var dispatcherType = typeof(IDispatcherWithCancellationToken);

        // Act & Assert
        Assert.Throws<ParameterCountMismatchException>(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithDispatcherHavingMoreParameters_ThrowsParameterCountMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerWithCancellationToken<IInputType>);
        var dispatcherType = typeof(IDispatcherWithThreeParameters);

        // Act & Assert
        Assert.Throws<ParameterCountMismatchException>(() => CrossValidateMethodParameters.Validate(handlerType, dispatcherType));
    }
}