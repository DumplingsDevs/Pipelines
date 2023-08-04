using System.Reflection;
using Pipelines.Builder.Validators.Shared.CompareTypes;
using Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.MethodResultTypes;

internal static class MethodResultTypesValidator
{
    internal static void Validate(MethodInfo methodToValidate, Type[] expectedResultTypes, Type handlerType)
    {
        var isVoidMethod = methodToValidate.IsVoidOrTaskReturnType();
        var methodReturnTypes = methodToValidate.GetReturnTypes();

        ValidateVoidMethod(isVoidMethod, expectedResultTypes, handlerType);

        if (isVoidMethod) return;

        CompareInputResultTypeCountWithHandler(expectedResultTypes.Length, methodReturnTypes.Count);

        ReturnTypesShouldHaveClassConstraintValidator.Validate(methodReturnTypes, handlerType);

        CompareInputResultTypesMatchWithHandler(expectedResultTypes, methodReturnTypes);
    }

    private static void CompareInputResultTypesMatchWithHandler(Type[] expectedResultTypes,
        List<Type> methodReturnTypes)
    {
        for (var i = 0; i < expectedResultTypes.Length; i++)
        {
            var expectedResultType = expectedResultTypes[i];
            var methodReturnType = methodReturnTypes[i];

            TypeCompatibilityValidator.Validate(expectedResultType, methodReturnType);
        }
    }

    private static void ValidateVoidMethod(bool isVoidMethod, Type[] expectedResultTypes, Type handlerType)
    {
        switch (isVoidMethod)
        {
            case true when expectedResultTypes.Any():
                throw new ExpectedMethodWithResultException(handlerType);

            case false when !expectedResultTypes.Any():
                throw new ExpectedVoidMethodException(handlerType);
        }
    }

    private static void CompareInputResultTypeCountWithHandler(int expectedResultTypesLength,
        int methodReturnTypesLength)
    {
        if (expectedResultTypesLength != methodReturnTypesLength)
        {
            throw new ResultTypeCountMismatchException(expectedResultTypesLength, methodReturnTypesLength);
        }
    }
}