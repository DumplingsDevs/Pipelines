using System.Reflection;

namespace Pipelines.Utils;

public static class EnumerableExtension
{
    public static IEnumerable<Type> WhereConstructorDoesNotHaveGenericParameter(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => !x.HasConstructorWithType(parameterType));
    }

    public static IEnumerable<Type> WhereConstructorHasCompatibleGenericType(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => x.ConstructorHasCompatibleGenericType(parameterType));
    }
}