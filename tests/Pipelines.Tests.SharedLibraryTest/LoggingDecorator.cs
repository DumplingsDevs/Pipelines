using Microsoft.Extensions.Logging;
using Pipelines.Tests.SharedLibraryTest.Types;

namespace Pipelines.Tests.SharedLibraryTest;

public class LoggingDecorator<TCommand, TResult> : IHandlerShared<TCommand, TResult>
    where TCommand : IInputShared<TResult> where TResult : class
{
    private readonly IHandlerShared<TCommand, TResult> _handler;
    private readonly ILogger<LoggingDecorator<TCommand, TResult>> _logger;

    public LoggingDecorator(IHandlerShared<TCommand, TResult> handler,
        ILogger<LoggingDecorator<TCommand, TResult>> logger)
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