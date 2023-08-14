using Microsoft.Extensions.Logging;

namespace Pipelines.Tests.UseCases.HandlerWithResult.Sample;

using Types;

public class LoggingDecorator<TCommand, TResult> : IHandler<TCommand, TResult>
    where TCommand : IInput<TResult> where TResult : class
{
    private readonly IHandler<TCommand, TResult> _handler;
    private readonly ILogger<LoggingDecorator<TCommand, TResult>> _logger;

    public LoggingDecorator(IHandler<TCommand, TResult> handler, ILogger<LoggingDecorator<TCommand, TResult>> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
    {
        _logger.Log(LogLevel.Information, "Executing handler for input {0}", typeof(TCommand));
        var result = await _handler.HandleAsync(request, token);
        _logger.Log(LogLevel.Information, "Executed handler for input {0}", typeof(TCommand));

        return result;
    }
}