using Pipelines.Builder.Validators.CrossValidation.ResultType;
using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
using Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;
using Pipelines.Utils;

namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType;

public class CrossValidateResultTypesTests
{
    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherStringResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerStringResult<>);
        var dispatcherType = typeof(IDispatcherStringResult);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.DoesNotThrow(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherTaskStringResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskStringResult<>);
        var dispatcherType = typeof(IDispatcherTaskStringResult);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.DoesNotThrow(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherTaskGenericResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskGenericResult<,>);
        var dispatcherType = typeof(IDispatcherTaskGenericResult);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.DoesNotThrow(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithConstrainedResult_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithConstrainedResult<,>);
        var dispatcherType = typeof(IDispatcherTaskWithConstrainedResult);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.DoesNotThrow(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMatchingHandlerAndDispatcherWithTwoConstrainedResults_DoesNotThrowException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithTwoConstraintedResults<,,>);
        var dispatcherType = typeof(IDispatcherTaskWithTwoConstrainedResults);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.DoesNotThrow(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMismatchingResultTypeCount_ThrowsResultTypeCountMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskStringResult<>);
        var dispatcherType = typeof(IDispatcherTaskWithTwoConstrainedResults);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.Throws<ResultTypeCountMismatchException>(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMismatchingResultType_ThrowsTaskReturnTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerStringResult<>);
        var dispatcherType = typeof(IDispatcherTaskStringResult);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.Throws<TaskReturnTypeMismatchException>(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithVoidDispatcherAndHandlerWithResult_ThrowsReturnTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerStringResult<>);
        var dispatcherType = typeof(IDispatcherVoid);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.Throws<VoidAndValueMethodMismatchException>(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithGenericTypeCountMismatch_ThrowsGenericTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithTwoConstraintedResults<,,>);
        var dispatcherType = typeof(IDispatcherTaskWithClassConstraintedResults);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.Throws<GenericTypeCountMismatchException>(() =>
            CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo, dispatcherMethodInfo));
    }

    [Test]
    public void Validate_WithMismatchingGenericType_ThrowsGenericTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerTaskWithTwoConstraintedResults<,,>);
        var dispatcherType = typeof(IDispatcherTaskWithDifferentTwoConstraintedResults);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.Throws<GenericTypeMismatchException>(
            () => CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo,
                dispatcherMethodInfo));
    }
    
    [Test]
    public void Validate_WithMismatchingType_ThrowsGenericTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IHandlerIntResult<>);
        var dispatcherType = typeof(IDispatcherStringResult);
        var handlerMethodInfo = handlerType.GetFirstMethodInfo();
        var dispatcherMethodInfo = dispatcherType.GetFirstMethodInfo();

        // Act & Assert
        Assert.Throws<TypeMismatchException>(
            () => CrossValidateResultTypes.Validate(handlerType, dispatcherType, handlerMethodInfo,
                dispatcherMethodInfo));
    }
}