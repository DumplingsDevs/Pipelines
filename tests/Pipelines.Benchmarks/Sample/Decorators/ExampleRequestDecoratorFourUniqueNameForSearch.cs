using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class ExampleRequestDecoratorFourUniqueNameForSearch<TInput, TResult> : IRequestHandler<TInput, TResult> where TInput : IRequest<TResult>
{
    private readonly IRequestHandler<TInput, TResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorFourUniqueNameForSearch(
        IRequestHandler<TInput, TResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {
        _state.Status.Add(typeof(ExampleRequestDecoratorFourUniqueNameForSearch<,>).Name);

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(ExampleRequestDecoratorFourUniqueNameForSearch<,>).Name);

        return result;
    }
}