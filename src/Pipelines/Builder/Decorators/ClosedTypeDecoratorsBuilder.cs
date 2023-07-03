using System.Reflection;
using Pipelines.Builder.Interfaces;
using Pipelines.Utils;

namespace Pipelines.Builder.Decorators;

internal class ClosedTypeDecoratorsBuilder : IPipelineClosedTypeDecoratorBuilder
{
    private readonly Assembly[] _assemblies;
    private readonly Type _handlerType;
    private readonly List<Func<Type, bool>> _predicates = new();

    public ClosedTypeDecoratorsBuilder(Assembly[] assemblies, Type handlerType)
    {
        _assemblies = assemblies;
        _handlerType = handlerType;
    }

    public void WithNameContaining(string pattern)
    {
        bool Predicate(Type t) => t.Name.Contains(pattern, StringComparison.InvariantCultureIgnoreCase);
        _predicates.Add(Predicate);
    }

    public void WithImplementedInterface<T>()
    {
        bool Predicate(Type t) => t.GetInterfaces().Contains(typeof(T));
        _predicates.Add(Predicate);
    }

    public void WithInheritedClass<T>()
    {
        bool Predicate(Type t) => t.IsSubclassOf(typeof(T));
        _predicates.Add(Predicate);
    }

    public void WithAttribute<T>()
    {
        bool Predicate(Type t) => t.GetCustomAttribute(typeof(T)) != null;
        _predicates.Add(Predicate);
    }

    public void With(Func<Type, bool> func)
    {
        _predicates.Add(func);
    }

    public IEnumerable<Type> GetDecoratorTypes()
    {
        // TO DO - check if decorator implementers handler type 
        return _predicates.SelectMany(x => _assemblies.SelectMany(y => y.GetTypes().Where(x)))
            .WhereConstructorHasCompatibleGenericType(_handlerType);
    }
}