using Pipelines.Exceptions;

namespace Pipelines.Utils;

internal static class TypeNamespaceValidator
{
    internal static void Validate(Type type1, Type type2)
    {
        var handleInputNamespace = GetNamespace(type1);
        var inputTypeFullname = GetNamespace(type2);
            
        if (!handleInputNamespace.Equals(inputTypeFullname))
        {
            throw new NamespaceMismatchException(handleInputNamespace, inputTypeFullname);
        }

        string GetNamespace(Type type)
        {
            return type.FullName ?? type.Namespace + "." + type.Name;
        }
    }
}