namespace Pipelines.Tests.UseCases.VoidHandler.Sample;
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

    public void Handle(TCommand request)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        _handler.Handle(request);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);
    }
}