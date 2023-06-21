using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class TracingDecorator<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
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
        _state.Status.Add("TracingDecorator");

        Console.WriteLine($"[TracingDecorator] Start of handling {request.GetType().Name}");
        var result = await _handler.HandleAsync(request, token);
        Console.WriteLine($"[TracingDecorator] Stop of handling {request.GetType().Name}");

        _state.Status.Add("TracingDecorator");

        return result;
    }
}