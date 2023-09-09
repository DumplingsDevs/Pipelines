namespace Pipelines.Builder.Decorators.Attributes;

internal abstract class AttributeSorterBase<T> where T : Attribute
{
    public abstract IEnumerable<Type> SortEnumerable(IEnumerable<Type> enumerable);
}