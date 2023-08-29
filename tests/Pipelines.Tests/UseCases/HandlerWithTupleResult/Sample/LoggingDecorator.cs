namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;
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

    public (TResult,TResult2) HandleAsync(TInput input)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        var result = _handler.HandleAsync(input);

        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        return result;
    }
}