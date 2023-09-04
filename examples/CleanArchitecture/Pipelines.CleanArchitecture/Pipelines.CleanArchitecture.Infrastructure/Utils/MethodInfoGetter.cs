using System.Reflection;

namespace Pipelines.CleanArchitecture.Infrastructure.Utils;

internal static class MethodInfoGetter
{
    internal static MethodInfo? GetByName(object handler, string methodName)
    {
        var methods = handler.GetType().GetMethods();
        var handleMethod = methods.FirstOrDefault(x => x.Name == methodName);

        return handleMethod;
    }

    internal static MethodInfo? GetGenericByName(object handler, string methodName, params Type[] typeArguments)
    {
        var methodInfo = GetByName(handler, methodName);

        return methodInfo?.MakeGenericMethod(typeArguments);
    }
}