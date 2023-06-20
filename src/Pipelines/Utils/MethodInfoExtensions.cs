using System.Reflection;

namespace Pipelines.Utils;

public static class MethodInfoExtensions
{
    public static List<Type> GetReturnTypes(this MethodInfo methodInfo)
    {
        var returnTypes = new List<Type>();
        var returnType = methodInfo.ReturnType;

        if (returnType.IsGenericType)
        {
            var genericTypeDefinition = returnType.GetGenericTypeDefinition();

            if (genericTypeDefinition == typeof(Task<>))
            {
                var taskType = returnType.GetGenericArguments()[0];
                
                if (taskType.IsGenericType)
                {
                    returnTypes.AddRange(taskType.GetGenericArguments());
                }
                else
                {
                    returnTypes.Add(taskType);
                }
            }
            else if (genericTypeDefinition == typeof(Tuple<>))
            {
                returnTypes.AddRange(returnType.GetGenericArguments());
            }
            else
            {
                returnTypes.Add(returnType);
            }
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