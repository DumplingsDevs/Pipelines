using Pipelines.Tests.UseCases.HandlerWithTaskWithBigTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithBigTuple.Sample;

public class LoggingDecorator<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> : IHandler<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
    where TInput : IInput<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> 
    where TResult : class
    where TResult2 : class
    where TResult3 : class
    where TResult4 : class
    where TResult5 : class
    where TResult6 : class
    where TResult7 : class
{
    private readonly IHandler<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<(TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7)> HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,,,,,,>).Name);
        var result = await _handler.HandleAsync(request, token);
        _state.Status.Add(typeof(LoggingDecorator<,,,,,,,>).Name);

        return result;
    }
}