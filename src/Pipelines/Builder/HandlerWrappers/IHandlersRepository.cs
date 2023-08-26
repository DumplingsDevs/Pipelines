namespace Pipelines.Builder.HandlerWrappers;

public interface IHandlersRepository
{
    IReadOnlyList<Type> GetHandlers();
}