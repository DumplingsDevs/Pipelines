using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class TracingDecorator<TInput, TResult> : IRequestHandler<TInput, TResult> where TInput : IRequest<TResult> where TResult : class
{
    private readonly IRequestHandler<TInput, TResult> _handler;
    private readonly DecoratorsState _state;

    public TracingDecorator(IRequestHandler<TInput, TResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(TracingDecorator<,>).Name);

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(TracingDecorator<,>).Name);

        return result;
    }
}