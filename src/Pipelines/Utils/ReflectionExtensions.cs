namespace Pipelines.Utils;

internal static class ReflectionExtensions
{
    public static bool HasCompatibleGenericArguments(this Type type, Type secondType)
    {
        var genericArguments = type.GetGenericArguments();

        if (secondType.IsGenericType)
        {
            try
            {
                _ = secondType.MakeGenericType(genericArguments);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        // TO DO: Change First() find interface by handler type
        var firstInterface = secondType.GetInterfaces().First();
        var interfaceGenericArguments = firstInterface.GetGenericArguments();

        if (!AreTheSameTypes(genericArguments, interfaceGenericArguments))
        {
            return false;
        }

        var genericInterfaceType = firstInterface.GetGenericTypeDefinition();

        try
        {
            _ = genericInterfaceType.MakeGenericType(genericArguments);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static bool AreTheSameTypes(Type[] genericArguments, Type[] interfaceGenericArguments)
    {
        // TO DO - refactor to something better?
        foreach (var genericArgument in genericArguments)
        {
            var exists = interfaceGenericArguments.Any(x =>
                x.Namespace == genericArgument.Namespace && x.Name == genericArgument.Name);

            if (!exists)
            {
                return false;
            }
        }

        return true;
    }
}