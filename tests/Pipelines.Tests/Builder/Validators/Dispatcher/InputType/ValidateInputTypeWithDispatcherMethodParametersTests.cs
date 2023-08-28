using Pipelines.Builder.Validators.Dispatcher.InputType;
using Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions;
using Pipelines.Tests.Builder.Validators.Dispatcher.InputType.Types;

namespace Pipelines.Tests.Builder.Validators.Dispatcher.InputType;

public class ValidateInputTypeWithDispatcherMethodParametersTests
{
    [Test]
    public void Validate_WhenTypesAreNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => ValidateInputTypeWithDispatcherMethodParameters.Validate(null, null));
    }

    [Test]
    public void Validate_WhenFirstParameterTypeIsNull_ThrowsException()
    {
        //Arrange
        var dispatcherType = typeof(IDispatcher);
        
        //Act and Assert
        Assert.Throws<ArgumentNullException>(() => ValidateInputTypeWithDispatcherMethodParameters.Validate(null, dispatcherType));
    }
    
    [Test]
    public void Validate_WhenSecondParameterTypeIsNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => ValidateInputTypeWithDispatcherMethodParameters.Validate(typeof(ICommand), null));
    }

    [Test]
    public void Validate_WhenTypesAreEqual_DoesNotThrowException()
    {
        //Arrange
        var inputType = typeof(ICommand);
        var dispatcherType = typeof(IDispatcher);
        
        //Act and Assert
        ValidateInputTypeWithDispatcherMethodParameters.Validate(inputType, dispatcherType);
    }
    
    [Test]
    public void Validate_WhenTypesAreEqual_DispatcherWithGenericMethodParameter_DoesNotThrowException()
    {
        //Arrange
        var inputType = typeof(ICommand);
        var dispatcherType = typeof(IDispatcherWithGenericMethodParameter);
        
        //Act and Assert
        ValidateInputTypeWithDispatcherMethodParameters.Validate(inputType, dispatcherType);
    }

    [Test]
    public void Validate_WhenTypesAreNotEqual_ThrowsDispatcherMethodInputTypeMismatchException()
    {
        //Arrange
        var inputType = typeof(ICommand);
        var dispatcherType = typeof(IDispatcherWithIncorrectParameter);

        //Act and Assert
        Assert.Throws<DispatcherMethodInputTypeMismatchException>(() => ValidateInputTypeWithDispatcherMethodParameters.Validate(inputType, dispatcherType));
    }
}