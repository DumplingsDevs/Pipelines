using System.Reflection;

namespace Pipelines.Builder.Decorators.Attributes;

internal class AscendingAttributeSorterBase<T, TKey> : AttributeSorterBase<T> where T : Attribute
{
    private readonly Func<T, TKey> _func;

    public AscendingAttributeSorterBase(Func<T, TKey> func)
    {
        _func = func;
    }

    public override IEnumerable<Type> SortEnumerable(IEnumerable<Type> enumerable)
    {
        return enumerable.OrderBy(x => _func(x.GetCustomAttribute<T>()!));
    }
}