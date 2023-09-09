namespace Pipelines.Builder.Decorators;

internal abstract class DecoratorSorterBase
{
    public abstract IEnumerable<Type> Sort(IEnumerable<Type> enumerable);
}