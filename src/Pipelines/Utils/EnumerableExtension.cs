using System.Reflection;

namespace Pipelines.Utils;

public static class EnumerableExtension
{
    public static IEnumerable<Type> WhereConstructorDoesNotHaveParameter(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => !ConstructorHasType(x.GetConstructors(), parameterType));
    }

    public static IEnumerable<Type> WhereConstructorHasParameter(this IEnumerable<Type> source, Type parameterType)
    {
        return source.Where(x => ConstructorHasType(x.GetConstructors(), parameterType));
    }

    public static IEnumerable<Type> WhereConstructorHasCompatibleGenericType(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => ConstructorHasGenericType(x.GetConstructors(), parameterType));
    }

    private static bool ConstructorHasGenericType(ConstructorInfo[] constructors, Type parameterType)
    {
        return constructors
            .Select(constructorInfo => constructorInfo.GetParameters()
                .Any(x => x.ParameterType.HasCompatibleGenericArguments(parameterType)))
            .Any(hasType => hasType);
    }

    private static bool ConstructorHasType(ConstructorInfo[] constructors, Type type)
    {
        // TO DO: double check if that condition is correct: x.ParameterType.Namespace == type.Namespace && x.ParameterType.Name == type.Name
        return constructors
            .Select(constructorInfo => constructorInfo.GetParameters()
                .Any(x => x.ParameterType.Namespace == type.Namespace && x.ParameterType.Name == type.Name))
            .Any(hasType => hasType);
    }
}