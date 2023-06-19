using Pipelines.Builder.Validators;
using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Invalid;
using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Valid;

namespace Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods;

public class ValidateHandlerHandleMethodsTests
{
    [Test]
    [TestCase(typeof(ICommandHandlerWithResult<,>))]
    [TestCase(typeof(IVoidCommandHandler<>))]
    [TestCase(typeof(ICommandHandlerWithTwoResults<,,>))]
    public void Validate_GenericArgumentsMatchesHandlerMethod_Passes(Type handlerType)
    {
        // Act
        ValidateHandlerHandleMethod.Validate(handlerType);

        // Assert
        Assert.Pass(); // if no exception was thrown, the test passes
    }
    
    [Test]
    [TestCase(typeof(IReturnResultExpectedVoid<,>))]
    [TestCase(typeof(IReturnSingleValueExpectedTwo<,,>))]
    [TestCase(typeof(IReturnTwoValuesExpectedOne<,>))]
    [TestCase(typeof(IVoidWithExpectedResult<>))]
    public void Validate_GenericArgumentsNotMatchesHandlerMethod_ThrowsException(Type handlerType)
    {
        // Act & Assert
        Assert.Throws<Exception>(() =>
            ValidateHandlerHandleMethod.Validate(handlerType));
    }
}