using Pipelines.Builder.Validators;
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
    [TestCase(typeof(IReturnResultExpectedVoid<>), TestName = "IReturnResultExpectedVoid")]
    [TestCase(typeof(IReturnSingleValueExpectedTwo<,,>), TestName = "IReturnSingleValueExpectedTwo")]
    [TestCase(typeof(IReturnTwoValuesExpectedOne<,>), TestName = "IReturnTwoValuesExpectedOne")]
    [TestCase(typeof(IVoidWithExpectedResult<>), TestName = "IVoidWithExpectedResult")]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsException(Type handlerType)
    {
        // Act & Assert
        Assert.Throws<Exception>(() =>
            ValidateHandlerHandleMethod.Validate(handlerType));
    }
}