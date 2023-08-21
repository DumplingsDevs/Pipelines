# Code examples

In this section of the documentation, you'll find ready-to-copy examples of pipelines.

------
## Table of Content 

- [1. Pipeline registration](#1-pipeline-registration)
- [2. Async pipelines](#2-async-pipelines)
  - [2.1 Task result](#21-task-result)
  - [2.2 Generic Task result](#22-generic-task-result)
  - [2.3 Generic Task Tuple result](#23-generic-task-tuple-result)
  - [2.4 Multiple method parameters](#24-multiple-method-parameters)
  - [2.5 No generic result](#25-no-generic-result)
- [3. Sync pipelines](#3-sync-pipelines)
  - [3.1 Void Result](#31-void-result)
  - [3.2 Generic result](#32-generic-result)
  - [3.3 Generic Tuple result](#33-generic-tuple-result)
  - [3.4 No generic result](#34-no-generic-result)

------


## 1. Pipeline registration

```cs
var handlersAssembly = //Assembly where handlers assembly are implemented
var dispatcherAssembly = //Assembly where AddPipeline gets invoked
var decoratorsAssembly1 = //Assembly where decorators are implemented
var decoratorsAssembly2 = //Another Assembly where decorators are implemented

_services
    .AddPipeline()
    .AddInput(typeof(IInput<>))
            .AddHandler(typeof(IHandler<,>), handlersAssembly)
            .AddDispatcher<IDispatcher>(dispatcherAssembly)
            .WithOpenTypeDecorator(typeof(LoggingDecorator<,>))
            .WithClosedTypeDecorators(x =>
            {
                x.WithImplementedInterface<IDecorator>();
                x.WithInheritedClass<BaseDecorator>();
                x.WithAttribute<DecoratorAttribute>();
                x.WithNameContaining("ExampleRequestDecoratorFourUniqueNameForSearch");
            }, decoratorsAssembly1, decoratorsAssembly2);
              
```

## 2. Async Pipelines

### 2.1 Task result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput { }
```
```cs
// Handler Interface
public interface IHandler<in TCommand> where TCommand : IInput
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public Task SendAsync(IInput input, CancellationToken token);
}
```

<b> Example implementation </b>

```cs
// Input Implementation
public record ExampleInput(string Value) : IInput;
```

```cs
// Handler Implementation
public class ExampleHandler : IHandler<ExampleInput>
{
    public Task HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}

```

```cs
// Open Type Decorator Implementation
public class LoggingDecorator<TCommand> : IHandler<TCommand>
    where TCommand : IInput
{
    private readonly IHandler<TCommand> _handler;

    public LoggingDecorator(IHandler<TCommand> handler)
    {
        _handler = handler;
    }

    public Task HandleAsync(TCommand request, CancellationToken token)
    {
        // Add logic there

        var result = _handler.HandleAsync(request, token);

        // Add logic there

        return result;
    }
}

```

[Unit tests](../tests/Pipelines.Tests/UseCases/TaskVoidHandler/)

----

### 2.2 Generic Task result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput<TResult> where TResult: class{ } 
```
```cs
// Handler Interface
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
```

```cs
// Dispatcher Interface
public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
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
### 2.3 Generic Task Tuple result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class { } 
```
```cs
// Handler Interface
public interface IHandler<in TCommand, TResult, TResult2>
    where TCommand : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TCommand command, CancellationToken token);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token) where TResult : class where TResult2 : class;
}
```

<b> Example implementation </b>

```cs
// Input Implementation
public record ExampleInput(string Value) : IInput<ExampleCommandResult, ExampleCommandResultSecond>;
```

```cs
// Result Implementation
public record ExampleCommandResult(string Value);
public record ExampleCommandResultSecond(string Value);
```

```cs
// Open Type Decorator Implementation
public class LoggingDecorator<TCommand, TResult, TResult2> : IHandler<TCommand, TResult, TResult2>
    where TCommand : IInput<TResult,TResult2> where TResult : class where TResult2 : class
{
    private readonly IHandler<TCommand, TResult, TResult2> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TCommand, TResult, TResult2> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<(TResult,TResult2)> HandleAsync(TCommand request, CancellationToken token)
    {
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);
        var result = await _handler.HandleAsync(request, token);
        _state.Status.Add(typeof(LoggingDecorator<,,>).Name);

        return result;
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithTaskWithTuple/)

----
### 2.4 Multiple method parameters

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput<TResult> where TResult: class { }
```
```cs
// Handler Interface
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token, bool canDoSomething,
        Dictionary<string, string> fancyDictionary);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken t, bool canDoSomething,
        Dictionary<string, string> dictionary) where TResult : class;
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
// Handler Implementation
public class ExampleInputHandler : IInputHandler<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token, bool canDoSomething, Dictionary<string, string> fancyDictionary)
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

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken token, bool canDoSomething, Dictionary<string, string> fancyDictionary)
    {
        _logger.Log(LogLevel.Information, "Executing handler for input {0}", typeof(TCommand));
        var result = await _handler.HandleAsync(command, token, canDoSomething, fancyDictionary);
        _logger.Log(LogLevel.Information, "Executed handler for input {0}", typeof(TCommand));

        return result;
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithMultipleParameters/)

----
### 2.5 No generic result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput { }
```
```cs
// Handler Interface
public interface IHandler<in TCommand> where TCommand : IInput
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public Task<string> SendAsync(IInput inputWithResult, CancellationToken token);
}
```

<b> Example implementation </b>

```cs
// Input Implementation
public record ExampleInput(string Name, int Value) : IInput;
```

```cs
// Handler Implementation
public class ExampleHandler : IHandler<ExampleInput>
{
    public Task<string> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult($"It's working!, {input.Name}, {input.Value}");
    }
}
```

```cs
// Open Type Decorator Implementation
public class LoggingDecorator<TCommand> : IHandler<TCommand>
    where TCommand : IInput
{
    private readonly IHandler<TCommand> _handler;

    public LoggingDecorator(IHandler<TCommand> handler)
    {
        _handler = handler;
    }

    public Task<string> HandleAsync(TCommand request, CancellationToken token)
    {
        var result = _handler.HandleAsync(request, token);

        return result;
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/NotGenericResult/)


## 3. Sync Pipelines


----
### 3.1 Void Result

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
// Handler Implementation
```

```cs
// Open Type Decorator Implementation
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithResult/)

----
### 3.2 Generic result

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
// Handler Implementation
```

```cs
// Open Type Decorator Implementation
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithResult/)

----
### 3.3 Generic Tuple result

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
// Handler Implementation
```

```cs
// Open Type Decorator Implementation
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithResult/)


----
### 3.4 No generic result

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
// Handler Implementation
```

```cs
// Open Type Decorator Implementation
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithResult/)

