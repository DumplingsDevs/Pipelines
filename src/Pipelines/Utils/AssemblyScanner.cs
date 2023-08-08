using System.Reflection;

namespace Pipelines.Utils;

internal static class AssemblyScanner
{
    internal static List<Type> GetTypesBasedOnGenericType(Assembly[] assemblies, Type genericType)
    {
        var genericTypesList = new List<Type>();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType));

            genericTypesList.AddRange(types);
        }

        return genericTypesList;
    }
}