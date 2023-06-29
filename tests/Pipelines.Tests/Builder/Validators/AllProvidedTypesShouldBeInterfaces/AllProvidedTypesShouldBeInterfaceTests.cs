using Pipelines.Builder.Validators;
using Pipelines.Builder.Validators.Shared.InterfaceConstraint;
using Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions;
using Pipelines.Exceptions;
using Pipelines.Tests.Builder.Validators.AllProvidedTypesShouldBeInterfaces.Types;

namespace Pipelines.Tests.Builder.Validators.AllProvidedTypesShouldBeInterfaces;

public class AllProvidedTypeShouldInterfaceTests
{
    [Test]
    public void Validate_TypeIsInterface_NoExceptionThrown()
    {
        // Arrange
        var inputType = typeof(IExampleInterface);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsClass_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleClass);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }

    [Test]
    public void Validate_TypeIsRecord_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleRecord);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }

    [Test]
    public void Validate_TypeIsStruct_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleStruct);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }

    [Test]
    public void Validate_TypeIsEnum_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(ExampleEnum);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsString_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(string);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsStringClass_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(String);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }
    
    [Test]
    public void Validate_TypeIsInt_ExceptionThrown()
    {
        // Arrange
        var inputType = typeof(int);

        // Act & Assert
        Assert.Throws<ProvidedTypeIsNotInterfaceException>(() =>
            AllProvidedTypesShouldBeInterface.Validate(inputType));
    }
}