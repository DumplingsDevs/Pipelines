using System.Reflection;
using Pipelines.Builder.Interfaces;

namespace Pipelines.Builder.Decorators;

internal static class DecoratorsBuilder
{
    public static List<Type> BuildDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action, Type handlerType, params Assembly[] assemblies)
    {
        var builder = new ClosedTypeDecoratorsBuilder(assemblies, handlerType);

        action(builder);
        
        return builder.GetDecoratorTypes().ToList();
    }
}