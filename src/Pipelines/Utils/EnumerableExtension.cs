using System.Reflection;

namespace Pipelines.Utils;

public static class EnumerableExtension
{
    public static IEnumerable<Type> WhereConstructorDoesNotHaveParameter(this IEnumerable<Type> source, Type parameterType)
    {
        return source.Where(x => !ConstructorHasType(x.GetConstructors(), parameterType));
    }
    
    public static IEnumerable<Type> WhereConstructorHasParameter(this IEnumerable<Type> source, Type parameterType)
    {
        return source.Where(x => !ConstructorHasType(x.GetConstructors(), parameterType));
    }

    private static bool ConstructorHasType(ConstructorInfo[] constructors, Type type)
    {
        foreach (var constructorInfo in constructors)
        {
            var hasType = constructorInfo.GetParameters().Any(x => x.ParameterType.Namespace == type.Namespace);
            if (hasType)
            {
                return true;
            }
        }

        return false;
    }
}