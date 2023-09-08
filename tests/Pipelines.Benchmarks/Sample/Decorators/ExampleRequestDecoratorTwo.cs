using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class ExampleRequestDecoratorTwo<TInput, TResult> : BaseDecorator, IRequestHandler<TInput, TResult> where TInput : IRequest<TResult>
{
    private readonly IRequestHandler<TInput, TResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorTwo(
        IRequestHandler<TInput, TResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(ExampleRequestDecoratorTwo<,>).Name);

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(ExampleRequestDecoratorTwo<,>).Name);

        return result;
    }
}