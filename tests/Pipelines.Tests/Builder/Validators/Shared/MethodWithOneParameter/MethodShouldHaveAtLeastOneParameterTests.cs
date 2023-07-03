using Pipelines.Builder.Validators.Shared.MethodWithOneParameter;
using Pipelines.Builder.Validators.Shared.MethodWithOneParameter.Exceptions;
using Pipelines.Tests.Builder.Validators.Shared.MethodWithOneParameter.Types;

namespace Pipelines.Tests.Builder.Validators.Shared.MethodWithOneParameter;

public class MethodShouldHaveAtLeastOneParameterTests
{
    [Test]
    public void Validate_ShouldThrowException_WhenNoParameters()
    {
        // Arrange
        var typeToValidate = typeof(ITypeWithoutParameters);

        // Act and Assert
        Assert.Throws<MethodShouldHaveAtLeastOneParameterException>(() => MethodShouldHaveAtLeastOneParameter.Validate(typeToValidate));
    }

    [Test]
    public void Validate_ShouldNotThrowException_WhenThereAreParameters()
    {
        // Arrange
        var typeToValidate = typeof(ITypeWithParameters); 

        // Act
        MethodShouldHaveAtLeastOneParameter.Validate(typeToValidate);

        // Assert
        Assert.Pass();
    }
}