namespace Pipelines.Builder.Decorators;

public record DecoratorFilter(Func<Type, bool> Predicate)
{
    public Func<Type, bool> Predicate { get; } = Predicate;
}