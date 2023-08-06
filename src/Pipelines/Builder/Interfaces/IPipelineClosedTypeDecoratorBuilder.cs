namespace Pipelines.Builder.Interfaces;

public interface IPipelineClosedTypeDecoratorBuilder
{
    /// <summary>
    /// Add a predicate to filter types whose names contain a specific pattern.
    /// </summary>
    /// <param name="pattern">The string to seek.</param>
    void WithNameContaining(string pattern);
    
    /// <summary>
    /// Add a predicate to filter types that implement a specific interface.
    /// </summary>
    /// <typeparam name="T">The interface type to match.</typeparam>
    void WithImplementedInterface<T>();
    
    /// <summary>
    /// Add a predicate to filter types that inherit from a specific class.
    /// </summary>
    /// <typeparam name="T">The class type to match.</typeparam>
    void WithInheritedClass<T>();
    
    /// <summary>
    /// Add a predicate to filter types that have a specific attribute.
    /// </summary>
    /// <typeparam name="T">The attribute type to match.</typeparam>
    void WithAttribute<T>();
    
    /// <summary>
    /// Adds a custom function as a predicate to filter types.
    /// </summary>
    /// <param name="func">Function to filter the types.</param>
    void With(Func<Type, bool> func);
}