namespace Pipelines.Utils;

internal static class EnumerableExtension
{
    internal static IEnumerable<Type> WhereConstructorDoesNotHaveGenericParameter(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => !x.HasConstructorWithType(parameterType));
    }

    internal static IEnumerable<Type> WhereConstructorHasCompatibleGenericType(this IEnumerable<Type> source,
        Type parameterType)
    {
        return source.Where(x => x.ConstructorHasCompatibleGenericType(parameterType));
    }
}