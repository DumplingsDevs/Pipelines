using System.Reflection;
using Pipelines.Builder.Decorators.Attributes;
using Pipelines.Builder.Interfaces;
using Pipelines.Utils;

namespace Pipelines.Builder.Decorators;

internal class ClosedTypeDecoratorsBuilder : IPipelineClosedTypeDecoratorBuilder
{
    private readonly Assembly[] _assemblies;
    private readonly Type _handlerType;
    private readonly List<DecoratorFilter> _predicates = new();

    internal ClosedTypeDecoratorsBuilder(Assembly[] assemblies, Type handlerType)
    {
        _assemblies = assemblies;
        _handlerType = handlerType;
    }

    public void WithNameContaining(string pattern)
    {
        bool Predicate(Type t) => t.Name.Contains(pattern, StringComparison.InvariantCultureIgnoreCase);

        var filter = new DecoratorFilter(Predicate);

        _predicates.Add(filter);
    }

    public void WithImplementedInterface<T>()
    {
        bool Predicate(Type t) => t.GetInterfaces().Contains(typeof(T));

        var filter = new DecoratorFilter(Predicate);

        _predicates.Add(filter);
    }

    public void WithInheritedClass<T>()
    {
        bool Predicate(Type t) => t.IsSubclassOf(typeof(T));

        var filter = new DecoratorFilter(Predicate);

        _predicates.Add(filter);
    }

    public IDecoratorSorter<T> WithAttribute<T>() where T : Attribute
    {
        bool Predicate(Type t) => t.GetCustomAttribute(typeof(T)) != null;

        var sorter = new AttributeDecoratorSorter<T>();

        var filter = new DecoratorFilter(Predicate, sorter);

        _predicates.Add(filter);

        return sorter;
    }

    public void With(Func<Type, bool> func)
    {
        var filter = new DecoratorFilter(func);

        _predicates.Add(filter);
    }

    public IEnumerable<Type> GetDecoratorTypes()
    {
        return _predicates.SelectMany(x => _assemblies.SelectMany(y =>
            {
                var enumerable = y.GetTypes().Where(x.Predicate);

                if (x.DecoratorSorter is not null)
                {
                    enumerable = x.DecoratorSorter.Sort(enumerable);
                }

                return enumerable;
            }))
            .WhereConstructorHasCompatibleGenericType(_handlerType);
    }
}