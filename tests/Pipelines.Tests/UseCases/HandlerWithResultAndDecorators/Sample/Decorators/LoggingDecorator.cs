using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class LoggingDecorator<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
{
    private readonly IRequestHandler<TCommand, TResult> _handler;

    public LoggingDecorator(IRequestHandler<TCommand, TResult> handler)
    {
        _handler = handler;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
    {
        Console.WriteLine($"[LoggingDecorator] Start of handling {request.GetType().Name}");
        var result = await _handler.HandleAsync(request, token);
        Console.WriteLine($"[LoggingDecorator] Stop of handling {request.GetType().Name}");

        return result;
    }
}