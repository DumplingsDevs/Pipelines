using Pipelines.Builder.Validators;
using Pipelines.Exceptions;
using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Invalid;
using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Valid;

namespace Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods;

public class ValidateHandlerHandleMethodsTests
{
    [Test]
    [TestCase(typeof(ICommandHandlerWithResult<,>), TestName = "ICommandHandlerWithResult")]
    [TestCase(typeof(IVoidCommandHandler<>), TestName = "IVoidCommandHandler")]
    [TestCase(typeof(ICommandHandlerWithTwoResults<,,>), TestName = "ICommandHandlerWithTwoResults")]
    public void Validate_GenericArgumentsMatchesHandlerMethod_Passes(Type handlerType)
    {
        // Act
        ValidateHandlerHandleMethod.Validate(handlerType);

        
        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsExpectedResultException()
    {
        // Arrange
        Type handlerType = typeof(IReturnResultExpectedVoid<>);

        // Act & Assert
        Assert.Throws<ExpectedVoidMethodException>(() =>
            ValidateHandlerHandleMethod.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsExpectedVoidException()
    {
        // Arrange
        Type handlerType = typeof(IVoidWithExpectedResult<>);

        // Act & Assert
        Assert.Throws<ExpectedVoidMethodException>(() =>
            ValidateHandlerHandleMethod.Validate(handlerType));
    }

    [Test]
    [TestCase(typeof(IReturnTwoValuesExpectedOne<,>), TestName = "IReturnTwoValuesExpectedOne")]
    [TestCase(typeof(IReturnSingleValueExpectedTwo<,,>), TestName = "IReturnSingleValueExpectedTwo")]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsReturnCountMismatchException(Type handlerType)
    {
        // Act & Assert
        Assert.Throws<ResultTypeCountMismatchException>(() =>
            ValidateHandlerHandleMethod.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsReturnTypeMismatchException()
    {
        // Arrange
        Type handlerType = typeof(IReturnTwoValuesExpectedTwoTypeMismatch<,,>);

        // Act & Assert
        Assert.Throws<ReturnTypeMismatchException>(() =>
            ValidateHandlerHandleMethod.Validate(handlerType));
    }
}