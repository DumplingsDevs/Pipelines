using System.Reflection;

namespace Pipelines.Utils;

public static class EnumerableExtension
{
    public static IEnumerable<Type> WhereConstructorDoesNotHaveGenericParameter(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => !ConstructorHasType(x.GetConstructors(), parameterType));
    }

    public static IEnumerable<Type> WhereConstructorHasGenericParameter(this IEnumerable<Type> source, Type parameterType)
    {
        return source.Where(x => ConstructorHasType(x.GetConstructors(), parameterType));
    }

    public static IEnumerable<Type> WhereConstructorHasCompatibleGenericType(this IEnumerable<Type> source,
        Type parameterType)
    {
        //TO DO check if implements handler type
        return source.Where(x => ConstructorHasCompatibleGenericType(x.GetConstructors(), parameterType));
    }

    private static bool ConstructorHasCompatibleGenericType(ConstructorInfo[] constructors, Type parameterType)
    {
        return constructors
            .Select(constructorInfo => constructorInfo.GetParameters()
                .Any(x => x.ParameterType.HasCompatibleGenericArguments(parameterType)))
            .Any(hasType => hasType);
    }

    private static bool ConstructorHasType(ConstructorInfo[] constructors, Type type)
    {
        return constructors
            .Select(constructorInfo => constructorInfo.GetParameters()
                .Any(x => TypeNamespaceComparer.CompareWithoutFullName(x.ParameterType, type)))
            .Any(hasType => hasType);
    }
}