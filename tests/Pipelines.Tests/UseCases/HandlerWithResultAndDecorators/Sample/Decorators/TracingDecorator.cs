using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class TracingDecorator<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult> where TResult : class
{
    private readonly IRequestHandler<TCommand, TResult> _handler;
    private readonly DecoratorsState _state;

    public TracingDecorator(IRequestHandler<TCommand, TResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(TracingDecorator<,>).Name);

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(typeof(TracingDecorator<,>).Name);

        return result;
    }
}