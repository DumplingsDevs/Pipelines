using Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Types;

namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Sample;

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

    public Task HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        var result = _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        return result;
    }
}