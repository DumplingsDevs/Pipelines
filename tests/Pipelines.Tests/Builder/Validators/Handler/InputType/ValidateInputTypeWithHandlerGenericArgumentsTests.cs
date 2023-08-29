using Pipelines.Builder.Validators.Handler.InputType;
using Pipelines.Builder.Validators.Handler.InputType.Exceptions;
using Pipelines.Tests.Builder.Validators.Handler.InputType.Types;
using DiffNamespaceICommand =
    Pipelines.Tests.Builder.Validators.Handler.InputType.Types.DiffNamespace.ICommand;

namespace Pipelines.Tests.Builder.Validators.Handler.InputType;

public class ValidateInputTypeWithHandlerGenericArgumentsTests
{
    [Test]
    public void Validate_NullInputType_ThrowsArgumentNullException()
    {
        // Arrange
        Type inputType = null;
        var handlerType = typeof(IHandler<>);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_NullHandlerType_ThrowsArgumentNullException()
    {
        // Arrange
        var inputType = typeof(IInput);
        Type handlerType = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }
    
    [Test]
    public void Validate_InputWithOneResultTypeMethodMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(IHandlerWithResult<,>);
        var inputType = typeof(IInputWithResult<>);

        // Act
        ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_InputWithTwoResultTypesMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(IHandlerWithTwoResults<,,>);
        var inputType = typeof(IInputWithTwoResults<,>);

        // Act
        ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_InputWithoutResultMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(IHandler<>);
        var inputType = typeof(IInput);

        // Act
        ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

[Test]
    public void Validate_VoidCommandWithHandlerWithOneResult_ThrowsResultTypeCountMismatchException()
    {
        // Arrange
        var inputType = typeof(IInput);
        var handlerType = typeof(IHandlerWithResult<,>);

        // Act & Assert
        Assert.Throws<HandlerInputTypeMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandWithOneResultWithVoidHandler_ThrowsGenericArgumentsLengthMismatchException()
    {
        // Arrange
        var inputType = typeof(IInputWithResult<>);
        var handlerType = typeof(IHandler<>);

        // Act & Assert
        Assert.Throws<HandlerInputTypeMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandWithOneResultWithHandlerWithTwoResults_ThrowsGenericArgumentsLengthMismatchException()
    {
        // Arrange
        var inputType = typeof(IInputWithResult<>);
        var handlerType = typeof(IHandlerWithTwoResults<,,>);

        // Act & Assert
        Assert.Throws<HandlerInputTypeMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandWithTwoResultsWithHandlerWithOneResult_ThrowsGenericArgumentsLengthMismatchException()
    {
        // Arrange
        var inputType = typeof(IInputWithTwoResults<,>);
        var handlerType = typeof(IHandlerWithResult<,>);

        // Act & Assert
        Assert.Throws<HandlerInputTypeMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandFromDifferentNamespaceWithHandlerNotMatchInputType_ThrowsNamespaceMismatchException()
    {
        // Arrange
        var inputType = typeof(DiffNamespaceICommand);
        var handlerType = typeof(IHandler<>);

        // Act & Assert
        Assert.Throws<HandlerInputTypeMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }
    
    [Test]
    public void Validate_HandlerWithoutGenericArguments_ThrowsGenericArgumentsNotFoundException()
    {
        // Arrange
        var inputType = typeof(IInput);
        var handlerType = typeof(IHandlerWithoutGenericArguments);

        // Act & Assert
        Assert.Throws<GenericArgumentsNotFoundException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }
}