using System.Reflection;

namespace Pipelines.CleanArchitecture.Infrastructure.Utils;

internal static class MethodExecutor
{
    public static async Task<TResult> InvokeAsync<TResult>(MethodInfo methodInfo, object targetObject, params object[] parameters)
    {
        dynamic awaitable = methodInfo.Invoke(targetObject, parameters)!;
        await awaitable;

        return (TResult)awaitable.GetAwaiter().GetResult();
    }

    public static async Task InvokeAsync(MethodInfo methodInfo, object targetObject, params object[] parameters)
    {
        try
        {
            dynamic awaitable = methodInfo.Invoke(targetObject, parameters)!;
            await awaitable;
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException!;
        }
    }
    
    public static TResult Invoke<TResult>(MethodInfo methodInfo, object targetObject, params object[] parameters)
    {
        dynamic result = methodInfo.Invoke(targetObject, parameters)!;

        return (TResult)result;
    }
}