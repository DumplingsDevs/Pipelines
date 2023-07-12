using Pipelines.Builder.Validators.CrossValidation.ResultType;
using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType;

public class CrossValidateResultTypesTests
{
     [Test]
    public void Validate_WithMatchingHandlerAndDispatcherStringResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerStringResult<>);
        var dispatcherType = typeof(IDispatcherStringResult);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherTaskStringResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskStringResult<>);
        var dispatcherType = typeof(IDispatcherTaskStringResult);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherTaskGenericResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskGenericResult<,>);
        var dispatcherType = typeof(IDispatcherTaskGenericResult);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithConstrainedResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithConstrainedResult<,>);
        var dispatcherType = typeof(IDispatcherTaskWithConstrainedResult);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithTwoConstraintedResults_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithTwoConstraintedResults<,,>);
        var dispatcherType = typeof(IDispatcherTaskWithTwoConstraintedResults);

        // Act & Assert
        Assert.DoesNotThrow(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMismatchingResultTypeCount_ThrowsResultTypeCountMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskStringResult<>);
        var dispatcherType = typeof(IDispatcherTaskWithTwoConstraintedResults);

        // Act & Assert
        Assert.Throws<ResultTypeCountMismatchException>(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithMismatchingResultType_ThrowsTaskReturnTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerStringResult<>);
        var dispatcherType = typeof(IDispatcherTaskStringResult);

        // Act & Assert
        Assert.Throws<TaskReturnTypeMismatchException>(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }
    
    [Test]
    public void Validate_WithVoidDispatcherAndHandlerWithResult_ThrowsReturnTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerStringResult<>);
        var dispatcherType = typeof(IDispatcherVoid);

        // Act & Assert
        Assert.Throws<ReturnTypeMismatchException>(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }

    [Test]
    public void Validate_WithGenericTypeCountMismatch_ThrowsGenericTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithTwoConstraintedResults<,,>);
        var dispatcherType = typeof(IDispatcherTaskWithClassConstraintedResults);

        // Act & Assert
        Assert.Throws<GenericTypeCountMismatchException>(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }
    
    [Test]
    public void Validate_WithMismatchingGenericType_ThrowsGenericTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithTwoConstraintedResults<,,>);
        var dispatcherType = typeof(IDispatcherTaskWithDifferentTwoConstraintedResults);

        // Act & Assert
        Assert.Throws<GenericTypeMismatchException>(() => CrossValidateResultTypes.Validate(handlerType, dispatcherType));
    }
}