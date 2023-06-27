namespace Pipelines.Builder.Interfaces;

public interface IPipelineClosedTypeDecoratorBuilder
{
    void WithNamePattern(string pattern);
    void WithImplementedInterface<T>();
    void WithInheritedClass<T>();
    void WithAttribute<T>();
    void With(Func<Type, bool> func);
}