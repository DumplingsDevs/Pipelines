using System.Reflection;

namespace Pipelines.Builder.Decorators;

internal abstract class DecoratorSorterBase
{
    public abstract IEnumerable<Type> Sort(IEnumerable<Type> enumerable);
}

internal class DecoratorSorter<T> : DecoratorSorterBase, IDecoratorSorter<T> where T : Attribute
{
    private Func<T, object>? _sortFunc;
    private Func<T, object>? _sortDescendingFunc;

    public IDecoratorSorter<T> OrderBy(Func<T, object> func)
    {
        _sortFunc = func;
        return this;
    }

    public IDecoratorSorter<T> OrderByDescending(Func<T, object> func)
    {
        _sortDescendingFunc = func;
        return this;
    }

    public override IEnumerable<Type> Sort(IEnumerable<Type> enumerable)
    {
        if (_sortFunc is not null)
        {
            return enumerable.OrderBy(x => _sortFunc(x.GetCustomAttribute<T>()));
        }

        if (_sortDescendingFunc is not null)
        {
            return enumerable.OrderByDescending(x => _sortDescendingFunc(x.GetCustomAttribute<T>()));
        }

        return enumerable;
    }
}