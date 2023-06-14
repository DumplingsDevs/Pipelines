using Pipelines.Builder.Validators;
using Pipelines.Exceptions;
using Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod.Types;

namespace Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod;

public class ShouldImplementExactlySameHandleMethodsTests
{
    [Test]
    public void Validate_DifferentMethodName_Passes()
    {
        // Arrange
        var typeToValidateOne = typeof(IHandleMethod);
        var typeToValidateTwo = typeof(IHandleMethodButDifferentName);

        // Act
        ShouldImplementExactlySameHandleMethods.Validate(typeToValidateOne, typeToValidateTwo);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_MissingCancellationTokenParameter_ThrowsHandleMethodMismatchException()
    {
        // Arrange
        var typeToValidateOne = typeof(IHandleMethod);
        var typeToValidateTwo = typeof(IHandleWithoutCancellationToken);

        // Act & Assert
        Assert.Throws<HandleMethodMismatchException>(() =>
            ShouldImplementExactlySameHandleMethods.Validate(typeToValidateOne, typeToValidateTwo));
    }

    [Test]
    public void Validate_DifferentResultType_ThrowsHandleMethodMismatchException()
    {
        // Arrange
        var typeToValidateOne = typeof(IHandleMethod);
        var typeToValidateTwo = typeof(IDifferentResultType);

        // Act & Assert
        Assert.Throws<HandleMethodMismatchException>(() =>
            ShouldImplementExactlySameHandleMethods.Validate(typeToValidateOne, typeToValidateTwo));
    }

    [Test]
    public void Validate_DifferentInputType_ThrowsHandleMethodMismatchException()
    {
        // Arrange
        var typeToValidateOne = typeof(IHandleMethod);
        var typeToValidateTwo = typeof(IDifferentInputType);

        // Act & Assert
        Assert.Throws<HandleMethodMismatchException>(() =>
            ShouldImplementExactlySameHandleMethods.Validate(typeToValidateOne, typeToValidateTwo));
    }

    [Test]
    public void Validate_IdenticalHandleMethods_Passes()
    {
        // Arrange
        var typeToValidateOne = typeof(IHandleMethod);
        var typeToValidateTwo = typeof(IHandleMethod);

        // Act
        ShouldImplementExactlySameHandleMethods.Validate(typeToValidateOne, typeToValidateTwo);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }
}