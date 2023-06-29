using Pipelines.Builder.Validators;
using Pipelines.Builder.Validators.Handler.InputType;
using Pipelines.Builder.Validators.Handler.InputType.Exceptions;
using Pipelines.Exceptions;
using Pipelines.Tests.Builder.Validators.ValidateInputTypeWithHandlerGenericArgument.Types;
using DiffNamespaceICommand =
    Pipelines.Tests.Builder.Validators.ValidateInputTypeWithHandlerGenericArgument.Types.DiffNamespace.ICommand;

namespace Pipelines.Tests.Builder.Validators.ValidateInputTypeWithHandlerGenericArgument;

public class ValidateInputTypeWithHandlerGenericArgumentsTests
{
    [Test]
    public void Validate_NullInputType_ThrowsArgumentNullException()
    {
        // Arrange
        Type inputType = null;
        var handlerType = typeof(ICommandHandler<>);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_NullHandlerType_ThrowsArgumentNullException()
    {
        // Arrange
        var inputType = typeof(ICommand);
        Type handlerType = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }
    
    [Test]
    public void Validate_InputWithOneResultTypeMethodMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(ICommandHandlerWithResult<,>);
        var inputType = typeof(ICommandWithResult<>);

        // Act
        ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_InputWithTwoResultTypesMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(ICommandHandlerWithTwoResults<,,>);
        var inputType = typeof(ICommandWithTwoResults<,>);

        // Act
        ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_InputWithoutResultMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(ICommandHandler<>);
        var inputType = typeof(ICommand);

        // Act
        ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

[Test]
    public void Validate_VoidCommandWithHandlerWithOneResult_ThrowsResultTypeCountMismatchException()
    {
        // Arrange
        var inputType = typeof(ICommand);
        var handlerType = typeof(ICommandHandlerWithResult<,>);

        // Act & Assert
        Assert.Throws<GenericArgumentsLengthMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandWithOneResultWithVoidHandler_ThrowsGenericArgumentsLengthMismatchException()
    {
        // Arrange
        var inputType = typeof(ICommandWithResult<>);
        var handlerType = typeof(ICommandHandler<>);

        // Act & Assert
        Assert.Throws<GenericArgumentsLengthMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandWithOneResultWithHandlerWithTwoResults_ThrowsGenericArgumentsLengthMismatchException()
    {
        // Arrange
        var inputType = typeof(ICommandWithResult<>);
        var handlerType = typeof(ICommandHandlerWithTwoResults<,,>);

        // Act & Assert
        Assert.Throws<GenericArgumentsLengthMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandWithTwoResultsWithHandlerWithOneResult_ThrowsGenericArgumentsLengthMismatchException()
    {
        // Arrange
        var inputType = typeof(ICommandWithTwoResults<,>);
        var handlerType = typeof(ICommandHandlerWithResult<,>);

        // Act & Assert
        Assert.Throws<GenericArgumentsLengthMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }

    [Test]
    public void Validate_CommandFromDifferentNamespaceWithHandlerNotMatchInputType_ThrowsNamespaceMismatchException()
    {
        // Arrange
        var inputType = typeof(DiffNamespaceICommand);
        var handlerType = typeof(ICommandHandler<>);

        // Act & Assert
        Assert.Throws<NamespaceMismatchException>(() =>
            ValidateInputTypeWithHandlerGenericArguments.Validate(inputType, handlerType));
    }
}