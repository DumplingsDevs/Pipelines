using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Utils;

public static class IServiceCollectionExtensions
{
    public static void RegisterGenericTypesAsScoped(this IServiceCollection serviceCollection, IEnumerable<Type> types)
    {
        foreach (var t in types)
        {
            var interfaces = t.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType)
                {
                    serviceCollection.AddScoped(@interface, t);
                }
            }
        }
    }
}