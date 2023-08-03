using Pipelines.Builder.Validators.Dispatcher.ResultTypes;
using Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;
using Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes;

public class ValidateResultTypesWithDispatcherInputResultTypesTests
{
    [Test]
    public void Validate_GivenIDispatcherMismatchGenericMethodArgumentAndInputResultType_ThrowsException()
    {
        // Arrange
        var dispatcherType = typeof(IDispatcherMismatchGenericMethodArgumentAndInputResultType);
        var commandType = typeof(ICommand);

        // Act & Assert
        Assert.Throws<ExpectedVoidMethodException>(() =>
            ValidateResultTypesWithDispatcherInputResultTypes.Validate(commandType, dispatcherType));
    }

    [Test]
    public void Validate_GivenIDispatcherMismatchGenericMethodArgumentAndResultType_ThrowsException()
    {
        // Arrange
        var dispatcherType = typeof(IDispatcherMismatchGenericMethodArgumentAndResultType);
        var commandType = typeof(ICommandWithResult<>); 

        // Act & Assert
        Assert.Throws<ExpectedMethodWithResultException>(() =>
            ValidateResultTypesWithDispatcherInputResultTypes.Validate(commandType, dispatcherType));
    }

    [Test]
    public void Validate_GivenIDispatcherWithIntResult_InputResultTypeMismatch_CommandWithoutClassConstraint_ThrowsException()
    {
        // Arrange
        var dispatcherType = typeof(IDispatcherWithResult);
        var commandType = typeof(ICommandWithResult<int>); // Use a concrete type here

        // Act
        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>(() =>
            ValidateResultTypesWithDispatcherInputResultTypes.Validate(commandType, dispatcherType));

        // Assert
        Assert.Pass();
    }

    [Test]
    public void Validate_GivenIVoidDispatcher_DoesNotThrowException()
    {
        // Arrange
        var dispatcherType = typeof(IVoidDispatcher);
        var commandType = typeof(ICommand);

        // Act
        ValidateResultTypesWithDispatcherInputResultTypes.Validate(commandType, dispatcherType);

        // Assert
        Assert.Pass();
    }
}