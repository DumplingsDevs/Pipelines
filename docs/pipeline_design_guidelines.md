# Pipeline Design Guidelines

This page provides conventions and guidelines for defining and implementing Pipelines.

---

## Table of Contents

- [1. Pipelines conventions](#1-pipelines-conventions)
-----

## 1 . Pipelines conventions


- The `Input` must be the first parameter of the Dispatcher and Handler methods.

```cs
public interface IInput<TResult> where TResult: class{ } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

-------

- Result types for the `Dispatcher` and `Handler` must match. If `generic arguments` defined in `Input`,  they must also align with those in the `Dispatcher` and `Handler` 

```cs

public interface IInput<TResult> where TResult: class{ } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

```cs
public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class { } 

public interface IHandler<in TInput, TResult, TResult2>
    where TInput : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token) where TResult : class where TResult2 : class;
}
```

```cs
public interface IInput { }

public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task SendAsync(IInput input, CancellationToken token);
}
```

-------

- Method parameters for the `Dispatcher` and `Handler` must match.

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token, bool canDoSomething,
        Dictionary<string, string> fancyDictionary);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken t, bool canDoSomething,
        Dictionary<string, string> dictionary) where TResult : class;
}
```

-------

- If the `Dispatcher/Handler` returns a non-generic type, then the `Input` will not have any Generic Arguments.

```cs
public interface IInput { }

public interface IHandler<in TInput> where TInput : IInput
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInput inputWithResult, CancellationToken token);
}
```

-------

- The `Decorator` must implement the Handler interface and accept Handler as a constructor parameter.

```cs
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

public interface IInput<TResult> where TResult: class{ } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

-------