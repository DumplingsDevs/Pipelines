namespace Pipelines.Utils;

internal static class TypeNamespaceComparer
{
    internal static bool Compare(Type type1, Type type2)
    {
        var handleInputNamespace = GetNamespace(type1);
        var inputTypeFullname = GetNamespace(type2);

        return handleInputNamespace.Equals(inputTypeFullname);

        string GetNamespace(Type type)
        {
            return type.FullName ?? type.Namespace + "." + type.Name;
        }
    }
    
    internal static bool CompareWithoutFullName(Type type1, Type type2)
    {
        var handleInputNamespace = GetNamespace(type1);
        var inputTypeFullname = GetNamespace(type2);

        var result = handleInputNamespace.Equals(inputTypeFullname);
        return result;
        string GetNamespace(Type type)
        {
            return type.Namespace + "." + type.Name;
        }
    }
}