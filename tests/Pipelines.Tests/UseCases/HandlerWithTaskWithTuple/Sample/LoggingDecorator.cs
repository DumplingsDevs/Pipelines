using Pipelines.Tests.Builder.Decorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;
using Types;

public class LoggingDecorator<TInput, TResult, TResult2> : IHandler<TInput, TResult, TResult2>
    where TInput : IInput<TResult,TResult2>
{
    private readonly IHandler<TInput, TResult, TResult2> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<(TResult,TResult2)> HandleAsync(TInput input, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);
        var result = await _handler.HandleAsync(input, token);
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        return result;
    }
}