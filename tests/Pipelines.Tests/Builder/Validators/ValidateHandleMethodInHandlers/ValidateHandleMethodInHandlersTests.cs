using Pipelines.Builder.Validators;
using Pipelines.Tests.TestData;

namespace Pipelines.Tests.Builder.Validators.ValidateHandleMethodInHandlers;

public class ValidateHandleMethodInHandlersTests
{
    [Test]
    public void Validate_DifferentMethodName_Passes()
    {
        // Arrange
        var commandHandler = typeof(ICommandHandlerWithResult<>);
        var inputType = typeof(ICommandWithResult);

        // Act
        ValidateHandleMethodInHandler.Validate(inputType, commandHandler);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }
}