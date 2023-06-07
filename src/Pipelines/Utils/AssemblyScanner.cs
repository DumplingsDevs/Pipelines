using System.Reflection;

namespace Pipelines.Utils;

public static class AssemblyScanner
{
    public static List<Type> GetTypesBasedOnGenericType(Assembly assembly, Type genericType)
    {
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType));
        return types.ToList();
    }
}