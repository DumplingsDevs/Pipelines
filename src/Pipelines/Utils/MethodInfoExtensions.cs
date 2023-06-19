using System.Reflection;

namespace Pipelines.Utils;

public static class MethodInfoExtensions
{
    public static List<Type> GetReturnTypes(this MethodInfo methodInfo)
    {
        var returnTypes = new List<Type>();
        var returnType = methodInfo.ReturnType;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var taskType = returnType.GetGenericArguments()[0];
            returnTypes.Add(taskType);
        }
        else
        {
            returnTypes.Add(returnType);
        }

        return returnTypes;
    }
    
    public static bool IsVoidOrTaskReturnType(this MethodInfo methodInfo)
    {
        var returnType = methodInfo.ReturnType;
        return returnType == typeof(void) || returnType == typeof(Task);
    }
}