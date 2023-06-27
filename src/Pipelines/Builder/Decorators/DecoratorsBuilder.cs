using System.Reflection;
using Pipelines.Builder.Interfaces;

namespace Pipelines.Builder.Decorators;

internal class DecoratorsBuilder
{
    private readonly List<Type> _decorators = new();

    public List<Type> BuildDecorators() => _decorators;

    public void AddDecorator(Type genericDecorator)
    {
        _decorators.Add(genericDecorator);
    }

    public void AddDecorators(List<Type> decorators)
    {
        _decorators.AddRange(decorators);
    }

    public void AddDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action, Type handlerType, params Assembly[] assemblies)
    {
        var builder = new ClosedTypeDecoratorsBuilder(assemblies, handlerType);

        action(builder);
        
        _decorators.AddRange(builder.GetDecoratorTypes());
    }
}