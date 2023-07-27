using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class LoggingDecorator<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
{
    private readonly IRequestHandler<TCommand, TResult> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IRequestHandler<TCommand, TResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,>).Name);

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(LoggingDecorator<,>).Name);
        
        return result;
    }
}