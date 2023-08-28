using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Sample;

public class LoggingDecorator<TInput, TResult, TResult2, TResult3> : IHandler<TInput, TResult, TResult2, TResult3>
    where TInput : IInput<TResult, TResult2> where TResult : class where TResult2 : class where TResult3 : class
{
    private readonly IHandler<TInput, TResult, TResult2, TResult3> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2, TResult3> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public (TResult, TResult2, TResult3) HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,,>).Name);

        var result = _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<,,,>).Name);

        return result;
    }
}