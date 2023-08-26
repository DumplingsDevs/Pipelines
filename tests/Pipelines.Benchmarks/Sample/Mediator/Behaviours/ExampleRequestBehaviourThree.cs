using CommandLine.Text;
using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class ExampleRequestBehaviourThree<TCommand, TResult> : IPipelineBehavior<TCommand, TResult> where TCommand : MediatorExampleRequest200
{
    private readonly DecoratorsState _state;

    public ExampleRequestBehaviourThree(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(ExampleRequestBehaviourThree<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(ExampleRequestBehaviourThree<,>).Name);

        return response;
    }
}