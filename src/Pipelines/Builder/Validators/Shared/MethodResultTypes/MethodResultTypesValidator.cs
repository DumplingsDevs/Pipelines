using System.Reflection;
using Pipelines.Builder.Validators.Shared.CompareTypes;
using Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.MethodResultTypes;

internal static class MethodResultTypesValidator
{
    internal static void Validate(MethodInfo methodToValidate, Type[] expectedResultTypes, Type handlerType,
        Type expectedResultSourceType)
    {
        var isVoidMethod = methodToValidate.IsVoidOrTaskReturnType();
        var methodReturnTypes = methodToValidate.GetReturnTypes();

        ValidateVoidMethod(isVoidMethod, expectedResultTypes, handlerType, expectedResultSourceType);

        if (isVoidMethod) return;

        CompareInputResultTypeCountWithHandler(expectedResultTypes.Length, methodReturnTypes.Count, handlerType,
            expectedResultSourceType);

        ReturnTypesShouldBeClassOrHaveClassConstraintValidator.Validate(methodReturnTypes, handlerType);

        CompareInputResultTypesMatchWithHandler(expectedResultTypes, methodReturnTypes, handlerType,
            expectedResultSourceType);
    }

    private static void CompareInputResultTypesMatchWithHandler(Type[] expectedResultTypes,
        List<Type> methodReturnTypes, Type resultSourceType, Type expectedResultSourceType)
    {
        for (var i = 0; i < expectedResultTypes.Length; i++)
        {
            var expectedResultType = expectedResultTypes[i];
            var methodReturnType = methodReturnTypes[i];

            TypeCompatibilityValidator.Validate(expectedResultType, methodReturnType, resultSourceType,
                expectedResultSourceType);
        }
    }

    private static void ValidateVoidMethod(bool isVoidMethod, Type[] expectedResultTypes, Type handlerType,
        Type expectedResultSourceType)
    {
        switch (isVoidMethod)
        {
            case true when expectedResultTypes.Any():
                throw new ExpectedMethodWithResultException(handlerType, expectedResultSourceType);

            case false when !expectedResultTypes.Any():
                throw new ExpectedVoidMethodException(handlerType, expectedResultSourceType);
        }
    }

    private static void CompareInputResultTypeCountWithHandler(int expectedResultTypesLength,
        int methodReturnTypesLength, Type resultSourceType, Type expectedResultSourceType)
    {
        if (expectedResultTypesLength != methodReturnTypesLength)
        {
            throw new ResultTypeCountMismatchException(expectedResultTypesLength, methodReturnTypesLength,
                resultSourceType, expectedResultSourceType);
        }
    }
}