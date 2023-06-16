using Pipelines.Builder.Validators;
using Pipelines.Exceptions;
using Pipelines.Tests.Builder.Validators.ExactlyOneHandleMethodShouldBeDefineds.Types;

namespace Pipelines.Tests.Builder.Validators.ExactlyOneHandleMethodShouldBeDefineds;

public class ExactlyOneHandleMethodShouldBeDefinedTests
{
    [Test]
    public void Validate_NoValidHandleMethod_ThrowsHandlerMethodNotImplementedException()
    {
        // Arrange
        var inputType = typeof(ExampleClass);
        var typeToValidate = typeof(NoValidHandleMethodClass);

        // Act & Assert
        Assert.Throws<HandlerMethodNotImplementedException>(() =>
            ExactlyOneHandleMethodShouldBeDefined.Validate(inputType, typeToValidate));
    }

    [Test]
    public void Validate_OneValidHandleMethod_Passes()
    {
        // Arrange
        var inputType = typeof(ExampleClass);
        var typeToValidate = typeof(TwoMethodWithOneValidHandleMethodClass);

        // Act
        ExactlyOneHandleMethodShouldBeDefined.Validate(inputType, typeToValidate);

        // Assert
        Assert.Pass(); 
    }

    [Test]
    public void Validate_MultipleValidHandleMethods_ThrowsMultipleHandlerMethodsException()
    {
        // Arrange
        var inputType = typeof(ExampleClass);
        var typeToValidate = typeof(MultipleValidHandleMethodClass);

        // Act & Assert
        Assert.Throws<MultipleHandlerMethodsException>(() =>
            ExactlyOneHandleMethodShouldBeDefined.Validate(inputType, typeToValidate));
    }
    
    [Test]
    public void Validate_HandlerInterface_OneValidHandleMethod_Passes()
    {
        // Arrange
        var inputType = typeof(ICommand<>);
        var typeToValidate = typeof(ICommandHandler<,>);

        // Act
        ExactlyOneHandleMethodShouldBeDefined.Validate(inputType, typeToValidate);

        // Assert
        Assert.Pass(); 
    }
    
    [Test]
    public void Validate_DispatcherInterface_OneValidHandleMethod_Passes()
    {
        // Arrange
        var inputType = typeof(ICommand<>);
        var typeToValidate = typeof(ICommandDispatcher);

        // Act
        ExactlyOneHandleMethodShouldBeDefined.Validate(inputType, typeToValidate);

        // Assert
        Assert.Pass(); 
    }
}