namespace Pipelines.Builder.Decorators.Attributes;

internal class AttributeDecoratorSorter<T> : DecoratorSorterBase, IDecoratorSorter<T> where T : Attribute
{
    private AttributeSorterBase<T>? _sorter;

    public IDecoratorSorter<T> OrderBy<TKey>(Func<T, TKey> keySelector)
    {
        _sorter = new AscendingAttributeSorterBase<T, TKey>(keySelector);

        return this;
    }

    public IDecoratorSorter<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
    {
        _sorter = new DescendingAttributeSorterBase<T, TKey>(keySelector);

        return this;
    }

    public override IEnumerable<Type> Sort(IEnumerable<Type> enumerable)
    {
        return _sorter is not null ? _sorter.SortEnumerable(enumerable) : enumerable;
    }
}