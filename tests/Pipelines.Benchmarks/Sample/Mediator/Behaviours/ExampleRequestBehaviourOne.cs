using CommandLine.Text;
using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class ExampleRequestBehaviourOne<TCommand, TResult> : IPipelineBehavior<TCommand, TResult> where TCommand : MediatorExampleRequest
{
    private readonly DecoratorsState _state;

    public ExampleRequestBehaviourOne(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(ExampleRequestBehaviourOne<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(ExampleRequestBehaviourOne<,>).Name);

        return response;
    }
}