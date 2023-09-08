using CommandLine.Text;
using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class ExampleRequestBehaviourFive<TInput, TResult> : IPipelineBehavior<TInput, TResult>
{
    private readonly DecoratorsState _state;

    public ExampleRequestBehaviourFive(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TInput request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(ExampleRequestBehaviourFive<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(ExampleRequestBehaviourFive<,>).Name);

        return response;
    }
}