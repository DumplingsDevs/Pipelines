using Pipelines.Builder.Validators;
using Pipelines.Exceptions;
using Pipelines.Tests.Builder.Validators.InputTypeShouldBeClassOrRecords.Types;

namespace Pipelines.Tests.Builder.Validators.InputTypeShouldBeClassOrRecords;

public class InputTypeShouldBeClassOrRecordTests
{
    [Test]
    public void Validate_TypeIsClass_NoExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleClass);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }

    [Test]
    public void Validate_TypeIsRecord_NoExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleRecord);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }

    [Test]
    public void Validate_TypeIsStruct_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleStruct);

        // Act & Assert
        Assert.Throws<InvalidInputTypeException>(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }

    [Test]
    public void Validate_TypeIsEnum_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleEnum);

        // Act & Assert
        Assert.Throws<InvalidInputTypeException>(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsString_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(string);

        // Act & Assert
        Assert.Throws<InvalidInputTypeException>(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsStringClass_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(String);

        // Act & Assert
        Assert.Throws<InvalidInputTypeException>(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsInt_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(int);

        // Act & Assert
        Assert.Throws<InvalidInputTypeException>(() =>
            InputTypeShouldBeClassOrRecord.Validate(inputType));
    }
}