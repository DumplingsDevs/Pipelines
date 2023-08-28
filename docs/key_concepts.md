# Key Concepts

In this section of the documentation, you'll learn about the core components of our tool, including inputs, handlers, dispatchers, and decorators. Additionally, we'll explain how 'Pipeline' manages multiple handlers for a single input.

-----

## Table of Content 

- [1. Building blocks](#1-building-blocks)
  - [1.1 Input](#11-input)
  - [1.2 Handler](#12-handler)
  - [1.3 Dispatcher](#13-dispatcher)
  - [1.4 Decorators](#14-decorators)
- [2. Multiple handlers for same Input](#2-multiple-handlers-for-same-input)
- [3. Execution Flow](#3-execution-flow)
------

## 1 Building blocks
### 1.1 Input 
First method parameter in the Handler and Dispatcher methods, guiding the identification of the appropriate Handler. Pipelines supports handling with both void and results.

Generic Arguments defines result types. `Pipelines` supports handling with both void and results.

Examples: 
```cs
public interface IInput {} 
public interface IInput<TResult> where TResult: class{ } 
public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class { } 
```

### 1.2 Handler
The epicenter of your application logic. Handlers can yield synchronous (like void or simple types) or asynchronous results.

In case, when all handlers will return exactly same return type, you don't need to define it on Input Generic Arguments. 

Examples: 
```cs
public interface IHandler<in TInput> where TInput : IInput
{
    public void Handle(TInput command, CancellationToken token);
}

public interface IHandler<in TInput> where TInput : IInput
{
    public string Handle(TInput command, CancellationToken token);
}

public interface IHandler<in TInput, TResult, TResult2> where TInput : IInput<TResult, TResult2>
{
    public (TResult, TResult2) Handle(TInput command, CancellationToken token);
}
```

```cs
public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}

public interface IHandler<in TInput> where TInput : IInput
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}

public interface IHandler<in TInput, TResult, TResult2> where TInput : IInput<TResult, TResult2>
{
    public Task<(TResult, TResult2)> HandleAsync(TInput command, CancellationToken token);
}
```

### 1.3 Dispatcher
Dispatcher implementation is provided by `Pipelines`. Dispatcher ensures the right Handler (with decorators) is triggered based on the Input.

NOTE:
<i>
Dispatcher is not resposible for apply Decorators by its own because Decorators gets applied on handlers during registering handlers in Dependency Injection Container.
</i>

Examples:

```cs
public interface IDispatcher
{
    public void Send(IInput command);
}

public interface IDispatcher
{
    public string Send(IInput command);
}

public interface IDispatcher
{
    public (TResult, TResult2) Send<TResult, TResult2>(IInput<TResult, TResult2> command);
}
```

```cs
public interface IDispatcher
{
    public Task SendAsync(IInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(IInput<TResult, TResult2> command,
        CancellationToken token);
}
```

### 1.4 Decorators
Analogous to Middlewares in .NET. Decorators wrap around handlers to extend or modify their behavior. Think of them as layers of logic that execute before or after the handler. Decorators can be applied both for Open Types and Closed Types.

To implement new decorator there two things that needs to be done:
- implement Handler interface
- inject Handler instance in Constructor

When registering decorators, ensure the order of registration in the DI container is the same as the execution order you desire.
```
                          │   ┌─────────────────────┐   │
                          │   │ Decorator1:IHandler │   │
                          │   └─────────────────────┘   │
                          │                             │
                          │   ┌─────────────────────┐   │
                          │   │ Decorator2:IHandler │   │
                          │   └─────────────────────┘   │
             Registration │                             │  Invoke
              direction   │   ┌─────────────────────┐   │ direction
                          │   │ Decorator3:IHandler │   │
                          │   └─────────────────────┘   │
                          ▼                             │
                               ┌──────────────────┐     │
                               │ Handler:IHandler │     │
                               └──────────────────┘     │
                                                        │
                              ┌─────────────────────┐   │
                              │ Decorator3:IHandler │   │
                              └─────────────────────┘   │
                                                        │
                              ┌─────────────────────┐   │
                              │ Decorator2:IHandler │   │
                              └─────────────────────┘   │
                                                        │  
                              ┌─────────────────────┐   │ 
                              │ Decorator1:IHandler │   │
                              └─────────────────────┘   │
                                                        ▼
```

There is a lot of ways how to register Closed Types Decorators:

```cs
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

Open Type Decorator example:
```cs
public class LoggingDecorator<TInput, TResult> : IHandler<TInput, TResult> where TInput : IInput<TResult>
{
    private readonly IHandler<TInput, TResult> _handler;
   
    public LoggingDecorator(IHandler<TInput, TResult> handler)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {

        var result = await _handler.HandleAsync(request, token);

        return result;
    }
}
```

Closed Type Decorator example:
```cs
public class
    ExampleRequestDecoratorFive : IDecorator, IRequestHandler<ExampleRequest,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest, ExampleCommandResult> _handler;

    public ExampleRequestDecoratorFive(
        IRequestHandler<ExampleRequest, ExampleCommandResult> handler)
    {
        _handler = handler;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        var result = await _handler.HandleAsync(request, token);
        return result;
    }
```

## 2. Multiple handlers for same Input
In situations where you have multiple handlers for a single type of input, the dispatcher will execute each one, including their associated decorators. This use case is highly likely when you are creating a Pipeline for Domain Event Execution.


```
    Dispatcher Invocation │                             │  Handler1
              direction   │   ┌─────────────────────┐   │  Execution
                          │   │ Decorator :IHandler │   │
                          │   └─────────────────────┘   │
                          │                             │
                          │    ┌──────────────────┐     │
                          │    │ Handler1:IHandler│     │
                          │    └──────────────────┘     │
                          │                             │
                          │   ┌─────────────────────┐   │
                          │   │ Decorator:IHandler  │   │
                          │   └─────────────────────┘   ▼
                          │                              
                          │                                Handler2 
                          │   ┌─────────────────────┐   │  Execution
                          │   │ Decorator:IHandler  │   │
                          │   └─────────────────────┘   │
                          │                             │
                          │    ┌──────────────────┐     │
                          │    │ Handler2:IHandler│     │
                          │    └──────────────────┘     │
                          │                             │
                          │   ┌─────────────────────┐   │
                          │   │ Decorator:IHandler  │   │
                          │   └─────────────────────┘   │
                          ▼                             ▼
```

## 3. Execution Flow

1. An Input is dispatched using the Dispatcher.

2. The Dispatcher identifies the appropriate Handler based on the Input type.

3. If there are any Decorators, they get executed. They can apply logic before, after, or around the handler's execution.

4. The Handler executes the primary logic.

5. The result (if any) is returned to the caller.