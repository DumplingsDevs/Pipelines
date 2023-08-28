using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class TracingBehaviour<TInput, TResult> : IPipelineBehavior<TInput, TResult>
{
    private readonly DecoratorsState _state;

    public TracingBehaviour(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TInput request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(TracingBehaviour<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(TracingBehaviour<,>).Name);

        return response;
    }
}