using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;
using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions;
using Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod.Types;

namespace Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod;

public class ExactlyOneHandleMethodShouldBeDefinedTests
{
    private static readonly Type InputType = typeof(ICommand<>);

    [Test]
    public void Validate_OneValidHandleMethod_Passes()
    {
        // Arrange
        var typeToValidate = typeof(ISingleMethod<,>);

        // Act
        ExactlyOneHandleMethodShouldBeDefined.Validate(InputType, typeToValidate);

        // Assert
        Assert.Pass();
    }

    [Test]
    public void Validate_MultipleValidHandleMethods_ThrowsMultipleHandlerMethodsException()
    {
        // Arrange
        var typeToValidate = typeof(IMultipleHandleMethod);

        // Act & Assert
        Assert.Throws<MultipleHandlerMethodsException>(() =>
            ExactlyOneHandleMethodShouldBeDefined.Validate(InputType, typeToValidate));
    }

    [Test]
    public void Validate_InterfaceWithoutMethod_OneValidHandleMethod_ThrowsHandlerMethodNotImplementedException()
    {
        // Arrange
        var typeToValidate = typeof(INoMethod<,>);

        // Act & Assert
        Assert.Throws<HandlerMethodNotFoundException>(() =>
            ExactlyOneHandleMethodShouldBeDefined.Validate(InputType, typeToValidate));
    }
}