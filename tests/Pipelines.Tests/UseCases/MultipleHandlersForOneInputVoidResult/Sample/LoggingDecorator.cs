namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Sample;
using Types;

public class LoggingDecorator<TInput> : ICommandHandler<TInput>
    where TInput : ICommand
{
    private readonly ICommandHandler<TInput> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(ICommandHandler<TInput> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public void HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);
    }
}