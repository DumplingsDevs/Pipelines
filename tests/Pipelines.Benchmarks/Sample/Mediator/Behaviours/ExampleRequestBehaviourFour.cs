using CommandLine.Text;
using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class ExampleRequestBehaviourFour<TInput, TResult> : IPipelineBehavior<TInput, TResult> where TInput : MediatorExampleRequest200
{
    private readonly DecoratorsState _state;

    public ExampleRequestBehaviourFour(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TInput request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(ExampleRequestBehaviourFour<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(ExampleRequestBehaviourFour<,>).Name);

        return response;
    }
}