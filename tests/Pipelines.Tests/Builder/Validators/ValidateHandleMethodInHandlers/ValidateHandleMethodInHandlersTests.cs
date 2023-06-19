using Pipelines.Builder.Validators;
using Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers.Types;
using DiffNamespaceICommand =
    Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers.Types.DiffNamespace.ICommand;

namespace Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers;

public class ValidateHandleMethodInHandlersTests
{
    [Test]
    public void Validate_InputWithOneResultTypeMethodMatchesHandlerMethod_Passes()
    {
        // Arrange
        var commandHandler = typeof(ICommandHandlerWithResult<,>);
        var inputType = typeof(ICommandWithResult<>);

        // Act
        ValidateHandleMethodInHandler.Validate(inputType, commandHandler);

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
        ValidateHandleMethodInHandler.Validate(inputType, commandHandler);

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
        ValidateHandleMethodInHandler.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    [TestCase(typeof(ICommand), typeof(ICommandHandlerWithResult<,>))]
    [TestCase(typeof(ICommandWithResult<>), typeof(ICommandHandler<>))]
    [TestCase(typeof(ICommandWithResult<>), typeof(ICommandHandlerWithTwoResults<,,>))]
    [TestCase(typeof(ICommandWithTwoResults<,>), typeof(ICommandHandlerWithResult<,>))]
    [TestCase(typeof(DiffNamespaceICommand), typeof(ICommandHandler<>))]
    public void Validate_InputNotMatchHandlerMethod_ThrowException(Type inputType, Type handlerType)
    {
        // Act & Assert
        Assert.Throws<Exception>(() =>
            ValidateHandleMethodInHandler.Validate(inputType,
                handlerType)); // if no exception was thrown, the test passes
    }
}