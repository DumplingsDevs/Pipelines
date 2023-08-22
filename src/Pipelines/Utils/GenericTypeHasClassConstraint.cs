using System.Reflection;

namespace Pipelines.Utils;

internal static class GenericTypeHasClassConstraint
{
    internal static bool Check(Type type)
    {
        if (!type.IsGenericParameter) return false;
        var constraints = type.GenericParameterAttributes;
        
        return constraints.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint);
    }
}