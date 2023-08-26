namespace Pipelines.Builder.HandlerWrappers;

internal class HandlersRepository : IHandlersRepository
{
    private readonly List<Type> _handlers;
    public HandlersRepository(List<Type> handlers)
    {
        _handlers = handlers;
    }
    public IReadOnlyList<Type> GetHandlers()
    {
        return _handlers;
    }
}
