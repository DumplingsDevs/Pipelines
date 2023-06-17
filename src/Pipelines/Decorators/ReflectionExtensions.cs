using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Internal;

namespace Pipelines.Decorators;

internal static class ReflectionExtensions
{
    public static bool HasCompatibleGenericArguments(this Type type, Type genericTypeDefinition)
    {
        var genericArguments = type.GetGenericArguments();
        try
        {
            _ = genericTypeDefinition.MakeGenericType(genericArguments);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

}
