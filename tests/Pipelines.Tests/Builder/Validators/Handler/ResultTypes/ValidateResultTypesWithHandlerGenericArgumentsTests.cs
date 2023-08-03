using Pipelines.Builder.Validators.Handler.ResultTypes;
using Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.OnlyOneResultTypeOrVoid.Exceptions;
using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;
using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;

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
    // [TestCase(typeof(ICommandHandlerWithTwoResults<,,>), TestName = "ICommandHandlerWithTwoResults")] //
    //After we will remove limit to to max one result, this example above should be uncommented
    public void Validate_GenericArgumentsMatchesHandlerMethod_Passes(Type handlerType)
    {
        // Act
        ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }

    [Test]
    [TestCase(typeof(ICommandHandlerWithTwoResults<,,>), TestName = "ICommandHandlerWithTwoResults")]
    public void Validate_MoreThanOneResultType_Throws(Type handlerType)
    {
        // Assert
        Assert.Throws<MethodShouldNotReturnMoreThanOneResultException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsExpectedResultException()
    {
        // Arrange
        Type handlerType = typeof(IReturnResultExpectedVoid<>);

        // Act & Assert
        Assert.Throws<ExpectedVoidMethodException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsExpectedVoidException()
    {
        // Arrange
        Type handlerType = typeof(IVoidWithExpectedResult<,>);

        // Act & Assert
        Assert.Throws<ExpectedMethodWithResultException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    [TestCase(typeof(IReturnTwoValuesExpectedOne<,>), TestName = "IReturnTwoValuesExpectedOne")]
    // [TestCase(typeof(IReturnSingleValueExpectedTwo<,,>), TestName = "IReturnSingleValueExpectedTwo")] //After we will remove limit to to max one result, ResultTypeCountMismatchException should be validate
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsReturnCountMismatchException(Type handlerType)
    {
        // Act & Assert
        //After we will remove limit to to max one result, this validator should throw ResultTypeCountMismatchException
        Assert.Throws<MethodShouldNotReturnMoreThanOneResultException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }

    [Test]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsReturnTypeMismatchException()
    {
        // Arrange
        Type handlerType = typeof(IReturnTwoValuesExpectedTwoTypeMismatch<,,>);

        // Act & Assert
        //After we will remove limit to to max one result, this validator should throw ReturnTypeMismatchException
        Assert.Throws<MethodShouldNotReturnMoreThanOneResultException>(() =>
            ValidateResultTypesWithHandlerGenericArguments.Validate(handlerType));
    }
}