namespace Pipelines.Builder.HandlerWrappers;

internal class HandlersRepository : IHandlersRepository
{
    private readonly List<Type> _handlers;
    
    public Type DispatcherType { get; }
    
    public HandlersRepository(List<Type> handlers, Type dispatcherType)
    {
        _handlers = handlers;
        DispatcherType = dispatcherType;
    }
    public IReadOnlyList<Type> GetHandlers()
    {
        return _handlers;
    }
}
