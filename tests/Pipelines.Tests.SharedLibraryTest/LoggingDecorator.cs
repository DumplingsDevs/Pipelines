using Microsoft.Extensions.Logging;
using Pipelines.Tests.SharedLibraryTest.Types;

namespace Pipelines.Tests.SharedLibraryTest;

public class LoggingDecorator<TInput, TResult> : IHandlerShared<TInput, TResult>
    where TInput : IInputShared<TResult> where TResult : class
{
    private readonly IHandlerShared<TInput, TResult> _handler;
    private readonly ILogger<LoggingDecorator<TInput, TResult>> _logger;

    public LoggingDecorator(IHandlerShared<TInput, TResult> handler,
        ILogger<LoggingDecorator<TInput, TResult>> logger)
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