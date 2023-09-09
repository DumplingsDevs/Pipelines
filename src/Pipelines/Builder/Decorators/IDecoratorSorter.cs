namespace Pipelines.Builder.Decorators;

/// <summary>
/// Allows to add sorting logic.
/// </summary>
/// <typeparam name="TSource"></typeparam>
public interface IDecoratorSorter<out TSource>
{
    /// <summary>
    /// Sorts the decorators in ascending order according to a key.
    /// </summary>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <typeparam name="TKey">Selected key that will be used to sort decorators.</typeparam>
    /// <returns></returns>
    IDecoratorSorter<TSource> OrderBy<TKey>(Func<TSource, TKey> keySelector);
    
    /// <summary>
    /// Sorts the decorators in descending order according to a key.
    /// </summary>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <typeparam name="TKey">Selected key that will be used to sort decorators.</typeparam>
    /// <returns></returns>
    IDecoratorSorter<TSource> OrderByDescending<TKey>(Func<TSource, TKey> keySelector);
}