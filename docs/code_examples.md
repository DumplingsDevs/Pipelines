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
public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput input, CancellationToken token);
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
public class LoggingDecorator<TInput> : IHandler<TInput>
    where TInput : IInput
{
    private readonly IHandler<TInput> _handler;

    public LoggingDecorator(IHandler<TInput> handler)
    {
        _handler = handler;
    }

    public Task HandleAsync(TInput request, CancellationToken token)
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
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
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
public interface IHandler<in TInput, TResult, TResult2>
    where TInput : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TInput input, CancellationToken token);
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
public class LoggingDecorator<TInput, TResult, TResult2> : IHandler<TInput, TResult, TResult2>
    where TInput : IInput<TResult,TResult2> where TResult : class where TResult2 : class
{
    private readonly IHandler<TInput, TResult, TResult2> _handler;
    private readonly DecoratorsState _state;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<(TResult,TResult2)> HandleAsync(TInput request, CancellationToken token)
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
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token, bool canDoSomething,
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

    public async Task<TResult> HandleAsync(TInput input, CancellationToken token, bool canDoSomething, Dictionary<string, string> fancyDictionary)
    {
        _logger.Log(LogLevel.Information, "Executing handler for input {0}", typeof(TInput));
        var result = await _handler.HandleAsync(input, token, canDoSomething, fancyDictionary);
        _logger.Log(LogLevel.Information, "Executed handler for input {0}", typeof(TInput));

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
public interface IHandler<in TInput> where TInput : IInput
{
    public Task<string> HandleAsync(TInput input, CancellationToken token);
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
public class LoggingDecorator<TInput> : IHandler<TInput>
    where TInput : IInput
{
    private readonly IHandler<TInput> _handler;

    public LoggingDecorator(IHandler<TInput> handler)
    {
        _handler = handler;
    }

    public Task<string> HandleAsync(TInput request, CancellationToken token)
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
public interface IInput { }
```
```cs
// Handler Interface
public interface IHandler<in TInput> where TInput : IInput
{
    public void Handle(TInput input);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public void Send(IInput input);
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
    public void Handle(ExampleInput input)
    { }
}
```

```cs
// Open Type Decorator Implementation
public class LoggingDecorator<TInput> : IHandler<TInput>
    where TInput : IInput
{
    private readonly IHandler<TInput> _handler;

    public LoggingDecorator(IHandler<TInput> handler)
    {
        _handler = handler;
    }

    public void Handle(TInput request)
    {
        //Add Logic there

        _handler.Handle(request);

        //Add Logic there
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/VoidHandler/)

----
### 3.2 Generic result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput<TResult> where TResult: class{ } 
```
```cs
// Handler Interface
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public TResult Handle(TInput input);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public TResult Send<TResult>(IInput<TResult> input) where TResult : class;
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
public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public ExampleCommandResult Handle(ExampleInput input)
    {
        return new ExampleCommandResult(input.Value);
    }
}
```

```cs
// Open Type Decorator Implementation
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

    public TResult Handle(TInput request)
    {
        _logger.Log(LogLevel.Information, "Executing handler for input {0}", typeof(TInput));
        var result = _handler.Handle(request);
        _logger.Log(LogLevel.Information, "Executed handler for input {0}", typeof(TInput));

        return result;
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/SyncGenericResult/)

----
### 3.3 Generic Tuple result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class
{ }
```
```cs
// Handler Interface
public interface IHandler<in TInput, TResult, TResult2> where TInput : IInput<TResult, TResult2>
    where TResult : class where TResult2 : class
{
    public (TResult, TResult2) HandleAsync(TInput input, CancellationToken token);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public (TResult, TResult2) SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token) where TResult : class where TResult2 : class;
}
```

<b> Example implementation </b>

```cs
// Input Implementation
public record ExampleInput(string Value) : IInput<ExampleRecordCommandResult, ExampleCommandClassResult>;
```

```cs
// Result Implementation
public record ExampleRecordCommandResult(string Value);

public class ExampleCommandClassResult
{
    public ExampleCommandClassResult(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
```

```cs
// Handler Implementation
public class ExampleHandler : IHandler<ExampleInput, ExampleRecordCommandResult, ExampleCommandClassResult>
{
    public (ExampleRecordCommandResult, ExampleCommandClassResult) HandleAsync(ExampleInput input, CancellationToken token)
    {
        return (new ExampleRecordCommandResult(input.Value), new ExampleCommandClassResult("Value"));
    }
}
```

```cs
// Open Type Decorator Implementation
public class LoggingDecorator<TInput, TResult, TResult2> : IHandler<TInput, TResult, TResult2>
    where TInput : IInput<TResult,TResult2> where TResult : class where TResult2: class
{
    private readonly IHandler<TInput, TResult, TResult2> _handler;

    public LoggingDecorator(IHandler<TInput, TResult, TResult2> handler)
    {
        _handler = handler;
    }

    public (TResult,TResult2) HandleAsync(TInput request, CancellationToken token)
    {
        //Add logic there 

        var result = _handler.HandleAsync(request, token);

        //Add logic there 

        return result;
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/HandlerWithTupleResult/)


----
### 3.4 No generic result

<b> Interfaces </b>

```cs
// Input Interface
public interface IInput { }
```
```cs
// Handler Interface
public interface IHandler<in TInput> where TInput : IInput
{
    public string Handle(TInput input);
}
```
```cs
// Dispatcher Interface
public interface IDispatcher
{
    public string Send(IInput inputWithResult);
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
    public string Handle(ExampleInput input)
    {
        return $"It's working!, {input.Name}, {input.Value}";
    }
}
```

```cs
// Open Type Decorator Implementation
public class LoggingDecorator<TInput> : IHandler<TInput>
    where TInput : IInput
{
    private readonly IHandler<TInput> _handler;

    public LoggingDecorator(IHandler<TInput> handler)
    {
        _handler = handler;
    }

    public string Handle(TInput request)
    {
        //Add logic there

        var result = _handler.Handle(request);

        //Add logic there

        return result;
    }
}
```

[Unit tests](../tests/Pipelines.Tests/UseCases/SyncNotGenericResult/)

-----

