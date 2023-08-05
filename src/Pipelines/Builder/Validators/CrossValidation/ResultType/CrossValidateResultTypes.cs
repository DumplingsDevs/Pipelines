using System.Reflection;
using Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions;
using Pipelines.Builder.Validators.Shared.CompareTypes;
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
            throw new VoidAndValueMethodMismatchException(handlerType, dispatcherType);
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

        ValidateResultTypes(handlerResultTypes, dispatcherResultTypes, handlerType, dispatcherType);
    }

    //To check if result types are equate, we need to check generic constraints in case when it is generic type and compare namespaces, if it is not generic
    private static void ValidateResultTypes(List<Type> handlerResultTypes, List<Type> dispatcherResultTypes,
        Type handlerType, Type dispatcherType)
    {
        for (var i = 0; i < handlerResultTypes.Count; i++)
        {
            var handlerParam = handlerResultTypes[i];
            var dispatcherParam = dispatcherResultTypes[i];

            TypeCompatibilityValidator.Validate(handlerParam, dispatcherParam, handlerType, dispatcherType);
        }
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