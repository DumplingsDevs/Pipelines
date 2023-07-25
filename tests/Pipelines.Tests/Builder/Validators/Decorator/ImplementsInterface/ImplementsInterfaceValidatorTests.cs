using Pipelines.Builder.Validators.Decorator.ImplementsInterface;
using Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions;

namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface;
using Types;
using Samples;

[TestFixture]
public class ImplementsInterfaceValidatorTests
{
    [Test]
    [TestCase(typeof(DecoratorWithResult), typeof(ICommandHandler<,>))]
    [TestCase(typeof(VoidDecorator), typeof(IVoidCommandHandler<>))]
    public void Validate_WithCorrectDecoratorType_DoesNotThrowException(Type decoratorType, Type handlerInterfaceType)
    {
        // Act
        TestDelegate act = () => ImplementsInterfaceValidator.Validate(decoratorType, handlerInterfaceType);

        // Assert
        Assert.DoesNotThrow(act);
    }

    [Test]
    [TestCase(typeof(DecoratorWithResult), typeof(IVoidCommandHandler<>))]
    [TestCase(typeof(VoidDecorator), typeof(ICommandHandler<,>))]
    public void Validate_WithIncorrectDecoratorType_ThrowsInterfaceImplementationException(Type decoratorType, Type handlerInterfaceType)
    {
        // Act
        TestDelegate act = () => ImplementsInterfaceValidator.Validate(decoratorType, handlerInterfaceType);

        // Assert
        Assert.Throws<InterfaceImplementationException>(act);
    }
}