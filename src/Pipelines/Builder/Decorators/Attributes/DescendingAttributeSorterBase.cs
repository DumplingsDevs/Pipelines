using System.Reflection;

namespace Pipelines.Builder.Decorators.Attributes;

internal class DescendingAttributeSorterBase<T, TKey> : AttributeSorterBase<T> where T : Attribute
{
    private readonly Func<T, TKey> _func;

    public DescendingAttributeSorterBase(Func<T, TKey> func)
    {
        _func = func;
    }

    public override IEnumerable<Type> SortEnumerable(IEnumerable<Type> enumerable)
    {
        return enumerable.OrderByDescending(x => _func(x.GetCustomAttribute<T>()!));
    }
}