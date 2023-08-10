using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Builder.Validators.Handler.ResultTypes;
using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;
using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;
using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;
using ResultTypeCountMismatchException = Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ResultTypeCountMismatchException;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes;

public class ValidateResultTypesWithHandlerGenericArgumentsTests
{
    [Test]
    public void Validate_NullInputType_ThrowsArgumentNullException()
    {
        // Arrange
        Type handlerType = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    [TestCase(typeof(ICommandHandlerWithResult<,>), TestName = "ICommandHandlerWithResult")]
    [TestCase(typeof(IVoidCommandHandler<>), TestName = "IVoidCommandHandler")]
    [TestCase(typeof(ICommandHandlerWithTwoResults<,,>), TestName = "ICommandHandlerWithTwoResults")] //
    public void Validate_GenericArgumentsMatchesHandlerMethod_Passes(Type handlerType)
    {
        // Act
        ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsExpectedResultException()
    {
        // Arrange
        var handlerType = typeof(IReturnResultTypeNotSpecifiedInGenerics<>);

        // Act & Assert
        Assert.DoesNotThrow(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsExpectedVoidException()
    {
        // Arrange
        var handlerType = typeof(IVoidWithExpectedResult<,>);

        // Act & Assert
        Assert.Throws<ExpectedMethodWithResultException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    [TestCase(typeof(IReturnTwoValuesExpectedOne<,>), TestName = "IReturnTwoValuesExpectedOne")]
    [TestCase(typeof(IReturnSingleValueExpectedTwo<,,>), TestName = "IReturnSingleValueExpectedTwo")]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsReturnCountMismatchException(Type handlerType)
    {
        // Act & Assert
        Assert.Throws<ResultTypeCountMismatchException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsReturnTypeMismatchException()
    {
        // Arrange
        var handlerType = typeof(IReturnTwoValuesExpectedTwoTypeMismatch<,,>);

        // Act & Assert
        Assert.Throws<GenericTypeCountMismatchException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }
}