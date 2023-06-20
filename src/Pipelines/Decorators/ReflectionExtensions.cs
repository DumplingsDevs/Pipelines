using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Internal;

namespace Pipelines.Decorators;

internal static class ReflectionExtensions
{
    public static bool HasCompatibleGenericArguments(this Type type, Type genericTypeDefinition)
    {
        var genericArguments = type.GetGenericArguments();
        
        if (genericTypeDefinition.IsGenericType)
        {
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
        else
        {
            var genericInterfaceType = genericTypeDefinition.GetInterfaces().First().GetGenericTypeDefinition();
            
            try
            {
                _ = genericInterfaceType.MakeGenericType(genericArguments);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

    }

}
