using Pipelines.Tests.UseCases.HandlerWithBigTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithBigTuple.Sample;

public class LoggingDecorator<TCommand, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> : IHandler<TCommand, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
    where TCommand : IInput<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> 
    where TResult : class
    where TResult2 : class
    where TResult3 : class
    where TResult4 : class
    where TResult5 : class
    where TResult6 : class
    where TResult7 : class
{
    private readonly IHandler<TCommand, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TCommand, TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public (TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7) HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,,,,,,>).Name);
        var result = _handler.HandleAsync(request, token);
        _state.Status.Add(typeof(LoggingDecorator<,,,,,,,>).Name);

        return result;
    }
}