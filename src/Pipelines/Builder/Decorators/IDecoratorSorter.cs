namespace Pipelines.Builder.Decorators;

/// <summary>
/// Allows to add sorting logic using OrderBy function.
/// </summary>
/// <typeparam name="TSource"></typeparam>
public interface IDecoratorSorter<out TSource>
{
    IDecoratorSorter<TSource> OrderBy(Func<TSource, object> func);
    IDecoratorSorter<TSource> OrderByDescending(Func<TSource, object> func);
}