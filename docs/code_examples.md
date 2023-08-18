# Code examples
------

## Handler with Generic Task result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput<TResult>{ }
```
```cs
// Handler Interface
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
```

```cs
// Dispatcher Interface
public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}
```

<b> Example implementation </b>

```cs
// Input Implementation
public record ExampleInput(string Value) : IInput<ExampleCommandResult>;
```

```cs
// Result Implementation
public record ExampleCommandResult(string Value);
```

```cs
// Handler Type Implementation
public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
```

```cs
// Open Type Decorator Implementation
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
```
[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithResult/)

----
## Template for next examples

<b> Interfaces </b>

```cs
// Input Interface
```
```cs
// Handler Interface
```
```cs
// Dispatcher Interface
```

<b> Example implementation </b>

```cs
// Input Implementation
```

```cs
// Result Implementation
```

```cs
// Open Type Decorator Implementation
```