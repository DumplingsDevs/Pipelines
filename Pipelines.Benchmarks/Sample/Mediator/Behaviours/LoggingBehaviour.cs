using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator.Behaviours;

public class LoggingBehaviour<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
{
    private readonly DecoratorsState _state;

    public LoggingBehaviour(DecoratorsState state)
    {
        _state = state;
    }

    public async Task<TResult> Handle(TCommand request, RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        _state.Status.Add(typeof(LoggingBehaviour<,>).Name);

        var response = await next();

        _state.Status.Add(typeof(LoggingBehaviour<,>).Name);

        return response;
    }
}