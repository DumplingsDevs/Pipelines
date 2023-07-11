using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.CrossValidation.ResultType;

public class CrossValidateResultTypes
{
    public static void Validate(Type handlerType, Type dispatcherType)
    {
        var handlerResultTypes = GetResultTypes(handlerType);
        var dispatcherResultTypes = GetResultTypes(dispatcherType);

        if (handlerResultTypes.Count != dispatcherResultTypes.Count)
        {
            throw new ResultTypeCountMismatchException(handlerType, dispatcherType);
        }

        //To check if result types are equate, we need to check generic constraints in case when it is generic type and compare namespaces, if it is not generic
        for (var i = 0; i < handlerResultTypes.Count; i++)
        {
            var handlerParam = handlerResultTypes[i];
            var dispatcherParam = dispatcherResultTypes[i];

            if (handlerParam.IsGenericType != dispatcherParam.IsGenericType)
            {
                throw new ResultTypeMismatchException(handlerParam, dispatcherParam);
            }

            if (IsGenericTypes(handlerParam, dispatcherParam))
            {
                ValidateGenericType(handlerParam, dispatcherParam);
            }
            else
            {
                ValidateNonGenericType(handlerParam, dispatcherParam);
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

    private static void ValidateGenericType(Type handlerParam, Type dispatcherParam)
    {
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

    private static bool IsGenericTypes(Type handlerParam, Type dispatcherParam)
    {
        return handlerParam.IsGenericType && dispatcherParam.IsGenericType;
    }

    private static List<Type> GetResultTypes(Type type)
    {
        return type.GetMethods().First().GetReturnTypes();
    }
}