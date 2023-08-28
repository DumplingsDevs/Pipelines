namespace Pipelines.Tests.UseCases.NotGenericResult.Sample;
using Types;

public class LoggingDecorator<TInput> : IHandler<TInput>
    where TInput : IInput
{
    private readonly IHandler<TInput> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public Task<string> HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        var result = _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<>).Name);

        return result;
    }
}