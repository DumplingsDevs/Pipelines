using Pipelines.Builder.Validators.Decorator.Constructor;
using Pipelines.Builder.Validators.Decorator.Constructor.Exceptions;

namespace Pipelines.Tests.Builder.Validators.Decorator.Constructor;
using Types;
using Samples;

public class DecoratorConstructorValidatorTests
{
    [Test]
    [TestCase(typeof(DecoratorWithResult), typeof(ICommandHandler<,>))]
    [TestCase(typeof(VoidDecorator), typeof(IVoidCommandHandler<>))]
    public void Validate_WithCorrectDecoratorType_DoesNotThrowException(Type decoratorType, Type expectedParameterType)
    {
        // Act
        TestDelegate act = () => DecoratorConstructorValidator.Validate(decoratorType, expectedParameterType);

        // Assert
        Assert.DoesNotThrow(act);
    }

    [Test]
    [TestCase(typeof(DecoratorWithResult), typeof(IVoidCommandHandler<>))]
    [TestCase(typeof(VoidDecorator), typeof(ICommandHandler<,>))]
    [TestCase(typeof(VoidDecoratorWithoutCtor), typeof(IVoidCommandHandler<>))]
    public void Validate_WithIncorrectDecoratorType_ThrowsConstructorValidationException(Type decoratorType, Type expectedParameterType)
    {
        // Act
        TestDelegate act = () => DecoratorConstructorValidator.Validate(decoratorType, expectedParameterType);

        // Assert
        Assert.Throws<ConstructorValidationException>(act);
    }
}