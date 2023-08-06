namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Sample;
using Types;

public class LoggingDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(ICommandHandler<TCommand> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public void HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);
    }
}