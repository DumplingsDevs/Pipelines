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
}