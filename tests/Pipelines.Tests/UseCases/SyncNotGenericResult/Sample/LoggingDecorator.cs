namespace Pipelines.Tests.UseCases.SyncNotGenericResult.Sample;
using Types;

public class LoggingDecorator<TCommand> : IHandler<TCommand>
    where TCommand : IInput
{
    private readonly IHandler<TCommand> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TCommand> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public string Handle(TCommand request)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        var result = _handler.Handle(request);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        return result;
    }
}