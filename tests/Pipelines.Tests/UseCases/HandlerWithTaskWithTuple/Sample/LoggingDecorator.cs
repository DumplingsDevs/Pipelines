using Pipelines.Tests.Builder.Decorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;
using Types;

public class LoggingDecorator<TInput, TResult, TResult2> : IHandler<TInput, TResult, TResult2>
    where TInput : IInput<TResult,TResult2> where TResult : class where TResult2 : class
{
    private readonly IHandler<TInput, TResult, TResult2> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<(TResult,TResult2)> HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);
        var result = await _handler.HandleAsync(request, token);
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        return result;
    }
}