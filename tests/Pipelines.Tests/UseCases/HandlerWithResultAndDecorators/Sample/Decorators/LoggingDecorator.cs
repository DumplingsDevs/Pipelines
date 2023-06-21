using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class LoggingDecorator<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
{
    private readonly IRequestHandler<TCommand, TResult> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IRequestHandler<TCommand, TResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add("LoggingDecorator");

        Console.WriteLine($"[LoggingDecorator] Start of handling {request.GetType().Name}");
        var result = await _handler.HandleAsync(request, token);
        Console.WriteLine($"[LoggingDecorator] Stop of handling {request.GetType().Name}");

        _state.Status.Add("LoggingDecorator");
        
        return result;
    }
}