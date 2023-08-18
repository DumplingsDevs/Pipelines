namespace Pipelines.Tests.UseCases.TaskVoidHandler.Sample;
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

    public Task HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        var result = _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        return result;
    }
}