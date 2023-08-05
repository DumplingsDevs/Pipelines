using Pipelines.Tests.Builder.Validators.Shared.CompareTypes.Types;
using Pipelines.Builder.Validators.Shared.CompareTypes;
using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Tests.Builder.Validators.Shared.CompareTypes;

[TestFixture]
public class TypeCompatibilityValidatorTests
{
    [Test]
    public void Validate_SameGenericTypes_DifferentGenericName_ShouldNotThrow()
    {
        var dispatcherType = typeof(IQueryDispatcher);
        var inputType = typeof(IQuery<>);
        var dispatcherFirstReturnTypeFromMethod = dispatcherType.GetMethods().First().GetReturnTypes().First();
        var inputFirstReturnTypeFromGenerics = inputType.GetGenericArguments().First();

        Assert.DoesNotThrow(() => TypeCompatibilityValidator.Validate(inputFirstReturnTypeFromGenerics,
            dispatcherFirstReturnTypeFromMethod, inputType, dispatcherType));
    }

    [Test]
    public void Validate_DifferentGenericTypeCount_ShouldThrowGenericTypeCountMismatchException()
    {
        var dispatcherType = typeof(IQueryDispatcher);
        var inputType = typeof(IQueryWithInterfaceConstraint<>);
        var dispatcherFirstReturnTypeFromMethod = dispatcherType.GetMethods().First().GetReturnTypes().First();
        var inputFirstReturnTypeFromGenerics = inputType.GetGenericArguments().First();

        Assert.Throws<GenericTypeCountMismatchException>(() => TypeCompatibilityValidator.Validate(
            inputFirstReturnTypeFromGenerics,
            dispatcherFirstReturnTypeFromMethod, inputType, dispatcherType));
    }

    [Test]
    public void Validate_DifferentGenericType_ShouldThrowGenericTypeMismatchException()
    {
        var dispatcherType = typeof(IQueryDispatcherDifferentMarker);
        var inputType = typeof(IQueryWithInterfaceConstraint<>);
        var dispatcherFirstReturnTypeFromMethod = dispatcherType.GetMethods().First().GetReturnTypes().First();
        var inputFirstReturnTypeFromGenerics = inputType.GetGenericArguments().First();

        Assert.Throws<GenericTypeMismatchException>(() => TypeCompatibilityValidator.Validate(
            inputFirstReturnTypeFromGenerics,
            dispatcherFirstReturnTypeFromMethod, inputType, dispatcherType));
    }
    
    [Test]
    public void Validate_NonGenericTypeInDispatcher_ShouldThrowGenericTypeMismatchException()
    {
        var dispatcherType = typeof(IQueryDispatcherWithNoGenericResult);
        var inputType = typeof(IQuery<>);
        var dispatcherFirstReturnTypeFromMethod = dispatcherType.GetMethods().First().GetReturnTypes().First();
        var inputFirstReturnTypeFromGenerics = inputType.GetGenericArguments().First();

        Assert.Throws<IsGenericMismatchException>(() => TypeCompatibilityValidator.Validate(
            inputFirstReturnTypeFromGenerics,
            dispatcherFirstReturnTypeFromMethod, inputType, dispatcherType));
    }
}