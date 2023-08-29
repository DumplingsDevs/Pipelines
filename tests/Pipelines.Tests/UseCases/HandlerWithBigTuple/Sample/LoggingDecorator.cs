using Pipelines.Tests.UseCases.HandlerWithBigTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithBigTuple.Sample;

public class LoggingDecorator<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> : IHandler<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
    where TInput : IInput<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
{
    private readonly IHandler<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public (TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7) HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,,,,,,>).Name);
        var result = _handler.HandleAsync(request, token);
        _state.Status.Add(typeof(LoggingDecorator<,,,,,,,>).Name);

        return result;
    }
}