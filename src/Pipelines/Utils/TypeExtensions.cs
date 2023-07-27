using System.Reflection;

namespace Pipelines.Utils;

internal static class TypeExtensions
{
    internal static bool HasConstructorWithType(this Type type, Type parameterType)
    {
        var constructorInfos = type.GetConstructors();

        return constructorInfos
            .Select(constructorInfo => constructorInfo.ConstructorHasType(parameterType))
            .Any(hasType => hasType);
    }

    internal static bool ConstructorHasCompatibleGenericType(this Type type, Type parameterType)
    {
        return type.GetConstructors()
            .Select(constructorInfo => constructorInfo.GetParameters()
                .Any(x => x.ParameterType.HasCompatibleGenericArguments(parameterType)))
            .Any(hasType => hasType);
    }

    internal static MethodInfo GetFirstMethodInfo(this Type type)
    {
        return type.GetMethods().First();
    }

    internal static bool IsTuple(this Type type)
    {
        return type.IsGenericType
               && (type.GetGenericTypeDefinition() == typeof(Tuple<>)
                   || type.GetGenericTypeDefinition() == typeof(Tuple<,>)
                   || type.GetGenericTypeDefinition() == typeof(Tuple<,,>)
                   || type.GetGenericTypeDefinition() == typeof(Tuple<,,,>)
                   || type.GetGenericTypeDefinition() == typeof(Tuple<,,,,>)
                   || type.GetGenericTypeDefinition() == typeof(Tuple<,,,,,>)
                   || type.GetGenericTypeDefinition() == typeof(Tuple<,,,,,,>));
    }

    internal static bool IsValueTuple(this Type type)
    {
        return type.IsGenericType
               && (type.GetGenericTypeDefinition() == typeof(ValueTuple<>)
                   || type.GetGenericTypeDefinition() == typeof(ValueTuple<,>)
                   || type.GetGenericTypeDefinition() == typeof(ValueTuple<,,>)
                   || type.GetGenericTypeDefinition() == typeof(ValueTuple<,,,>)
                   || type.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,>)
                   || type.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,>)
                   || type.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,,>));
    }
}