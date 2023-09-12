namespace Pipelines.Builder.Decorators;

internal class DecoratorFilter
{
    public DecoratorFilter(Func<Type, bool> predicate, DecoratorSorterBase decoratorSorter)
    {
        Predicate = predicate;
        DecoratorSorter = decoratorSorter;
    }
    public DecoratorFilter(Func<Type, bool> predicate)
    {
        Predicate = predicate;
    }

    public Func<Type, bool> Predicate { get; }
    public DecoratorSorterBase? DecoratorSorter { get; } 
}