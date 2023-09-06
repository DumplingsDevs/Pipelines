using CommandLine.Text;
using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class ExampleRequestBehaviourTwo<TInput, TResult> : IPipelineBehavior<TInput, TResult>
{
    private readonly DecoratorsState _state;

    public ExampleRequestBehaviourTwo(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TInput request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(ExampleRequestBehaviourTwo<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(ExampleRequestBehaviourTwo<,>).Name);

        return response;
    }
}