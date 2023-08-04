using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.CompareTypes;

internal static class TypeCompatibilityValidator
{
    internal static void Validate(Type type1, Type type2)
    {
        if (IsGenericTypes(type1, type2))
        {
            ValidateGenericType(type1, type2);
        }
        else
        {
            ValidateNonGenericType(type1, type2);
        }
    }

    private static bool IsGenericTypes(Type handlerParam, Type dispatcherParam)
    {
        return handlerParam.IsGenericTypeParameter && dispatcherParam.IsGenericMethodParameter;
    }

    private static void ValidateGenericType(Type handlerParam, Type dispatcherParam)
    {
        //Important! "class" constraint is not exist in GenericParameterConstraints. For e.g. "struct" will be exists and will be with type "System.ValueType"
        var handlerParamGenericConstraints = handlerParam.GetGenericParameterConstraints();
        var dispatcherParamGenericConstraints = dispatcherParam.GetGenericParameterConstraints();

        if (handlerParamGenericConstraints.Length != dispatcherParamGenericConstraints.Length)
        {
            throw new GenericTypeCountMismatchException(handlerParam, dispatcherParam);
        }

        for (int i = 0; i < handlerParamGenericConstraints.Length; i++)
        {
            var handlerGenericType = handlerParamGenericConstraints[i];
            var dispatcherGenericType = dispatcherParamGenericConstraints[i];

            if (!TypeNamespaceComparer.Compare(handlerGenericType, dispatcherGenericType))
            {
                throw new GenericTypeMismatchException(handlerParam, dispatcherParam);
            }
        }
    }

    private static void ValidateNonGenericType(Type handlerParam, Type dispatcherParam)
    {
        if (!TypeNamespaceComparer.Compare(handlerParam, dispatcherParam))
        {
            throw new ResultTypeMismatchException(handlerParam, dispatcherParam);
        }
    }
}