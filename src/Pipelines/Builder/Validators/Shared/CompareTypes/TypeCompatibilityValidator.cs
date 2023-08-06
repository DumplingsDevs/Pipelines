using Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.CompareTypes;

internal static class TypeCompatibilityValidator
{
    internal static void Validate(Type type1, Type type2, Type sourceType1, Type sourceType2)
    {
        var isType1Generic = IsGenericType(type1);
        var isType2Generic = IsGenericType(type2);
        var compareGenericTypes = isType1Generic && isType2Generic;

        if (isType1Generic != isType2Generic)
        {
            throw new IsGenericMismatchException(type1, isType1Generic, sourceType1, type2, isType2Generic,
                sourceType2);
        }

        if (compareGenericTypes)
        {
            ValidateGenericType(type1, type2, sourceType1, sourceType2);
        }
        else
        {
            ValidateNonGenericType(type1, type2, sourceType1, sourceType2);
        }
    }

    private static bool IsGenericType(Type type)
    {
        return type.IsGenericType || type.IsGenericTypeParameter || type.IsGenericMethodParameter;
    }

    private static void ValidateGenericType(Type type1, Type type2, Type sourceType1, Type sourceType2)
    {
        //Important! "class" constraint is not exist in GenericParameterConstraints. For e.g. "struct" will be exists and will be with type "System.ValueType"
        var handlerParamGenericConstraints = type1.GetGenericParameterConstraints();
        var dispatcherParamGenericConstraints = type2.GetGenericParameterConstraints();

        if (handlerParamGenericConstraints.Length != dispatcherParamGenericConstraints.Length)
        {
            throw new GenericTypeCountMismatchException(type1, type2, sourceType1, sourceType2);
        }

        for (int i = 0; i < handlerParamGenericConstraints.Length; i++)
        {
            var handlerGenericType = handlerParamGenericConstraints[i];
            var dispatcherGenericType = dispatcherParamGenericConstraints[i];

            if (!TypeNamespaceComparer.Compare(handlerGenericType, dispatcherGenericType))
            {
                throw new GenericTypeMismatchException(type1, type2, sourceType1, sourceType2);
            }
        }
    }

    private static void ValidateNonGenericType(Type type1, Type type2, Type sourceType1, Type sourceType2)
    {
        if (!TypeNamespaceComparer.Compare(type1, type2))
        {
            throw new TypeMismatchException(type1, type2, sourceType1, sourceType2);
        }
    }
}