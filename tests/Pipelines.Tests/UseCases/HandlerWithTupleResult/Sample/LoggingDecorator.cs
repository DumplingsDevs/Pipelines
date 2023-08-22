namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;
using Types;

public class LoggingDecorator<TCommand, TResult, TResult2> : IHandler<TCommand, TResult, TResult2>
    where TCommand : IInput<TResult,TResult2> where TResult : class where TResult2: class
{
    private readonly IHandler<TCommand, TResult, TResult2> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TCommand, TResult, TResult2> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public (TResult,TResult2) HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        var result = _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        return result;
    }
}