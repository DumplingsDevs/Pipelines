using System.Reflection;

namespace Pipelines.Utils;

internal static class ConstructorInfoExtensions
{
    internal static bool ConstructorHasType(this ConstructorInfo constructorInfo, Type type)
    {
        return constructorInfo.GetParameters()
            .Any(x => TypeNamespaceComparer.CompareWithoutFullName(x.ParameterType, type));
    }
}