using Microsoft.Extensions.Logging;

namespace Pipelines.Tests.UseCases.HandlerWithResult.Sample;

using Types;

public class LoggingDecorator<TInput, TResult> : IHandler<TInput, TResult>
    where TInput : IInput<TResult> where TResult : class
{
    private readonly IHandler<TInput, TResult> _handler;
    private readonly ILogger<LoggingDecorator<TInput, TResult>> _logger;

    public LoggingDecorator(IHandler<TInput, TResult> handler, ILogger<LoggingDecorator<TInput, TResult>> logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {
        _logger.Log(LogLevel.Information, "Executing handler for input {0}", typeof(TInput));
        var result = await _handler.HandleAsync(request, token);
        _logger.Log(LogLevel.Information, "Executed handler for input {0}", typeof(TInput));

        return result;
    }
}