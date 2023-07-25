using System.Reflection;
using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.CrossValidation.ResultType;

public static class CrossValidateResultTypes
{
    public static void Validate(Type handlerType, Type dispatcherType, MethodInfo handlerHandleMethod,
        MethodInfo dispatcherHandleMethod)
    {
        var handlerMethod = GetMethodInfo(handlerType, handlerHandleMethod);
        var dispatcherMethod = GetMethodInfo(dispatcherType, dispatcherHandleMethod);

        if (handlerMethod.IsVoidOrTaskReturnType() != dispatcherMethod.IsVoidOrTaskReturnType())
        {
            throw new ReturnTypeMismatchException(handlerType, dispatcherType);
        }

        if (handlerMethod.IsGenericTaskReturnType() != dispatcherMethod.IsGenericTaskReturnType())
        {
            throw new TaskReturnTypeMismatchException(handlerType, dispatcherType);
        }
        
        var handlerResultTypes = GetResultTypes(handlerMethod);
        var dispatcherResultTypes = GetResultTypes(dispatcherMethod);

        if (handlerResultTypes.Count != dispatcherResultTypes.Count)
        {
            throw new ResultTypeCountMismatchException(handlerType, dispatcherType);
        }

        ValidateResultTypes(handlerResultTypes, dispatcherResultTypes);
    }

    //To check if result types are equate, we need to check generic constraints in case when it is generic type and compare namespaces, if it is not generic
    private static void ValidateResultTypes(List<Type> handlerResultTypes, List<Type> dispatcherResultTypes)
    {
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

    private static bool IsGenericTypes(Type handlerParam, Type dispatcherParam)
    {
        return handlerParam.IsGenericTypeParameter && dispatcherParam.IsGenericMethodParameter;
    }

    private static List<Type> GetResultTypes(MethodInfo methodInfo)
    {
        return methodInfo.GetReturnTypes();
    }
    
    private static MethodInfo GetMethodInfo(Type type, MethodInfo methodInfo)
    {
        return type.GetMethods().First(x => x.Equals(methodInfo));
    }
}