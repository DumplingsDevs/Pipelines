# Main Concepts

In this section of the documentation, you'll learn about the core components of our tool, including inputs, handlers, dispatchers, and decorators. Additionally, we'll explain how `Pipelines` manages multiple handlers for a single input.

-----

## Table of Content 

- [1. Building blocks](#1-building-blocks)
  - [1.1 Input](#11-input)
  - [1.2 Handler](#12-handler)
  - [1.3 Dispatcher](#13-dispatcher)
  - [1.4 Decorators](#14-decorators)
    - [1.4.1 WithAttributes](#141-withattribute)
- [2. Constraints on type parameters](#2-constraints-on-type-parameters)
- [3. Multiple handlers for same Input](#3-multiple-handlers-for-the-same-input)
- [4. Execution Flow](#4-execution-flow)
- [5. Pipelines rules](#5-pipelines-rules)
  - [Rule 1: Input Parameter Ordering](#rule-1-input-parameter-ordering)
  - [Rule 2: Result Type Alignment](#rule-2-result-type-alignment)
    - [Example 1: Single Result Type](#example-1-single-result-type)
    - [Example 2: Two Result Types](#example-2-two-result-types)
    - [Example 3: No Generic Arguments in Input](#example-3-no-generic-arguments-in-input)
  - [Rule 3: Method Parameter Matching](#rule-3-method-parameter-matching)
    - [Example: Two Parameters in the Handle Method](#example-two-parameters-in-the-handle-method)
    - [Example: Four Parameters in the Handle Method](#example-four-parameters-in-the-handle-method)
  - [Rule 4: Non-Generic Return with Non-Generic Input](#rule-4-non-generic-return-with-non-generic-input)
  - [Rule 5: Decorator Implementation](#rule-5-decorator-implementation)

------

## 1 Building blocks
### 1.1 Input
First method parameter in the Handler and Dispatcher methods, guiding the identification of the appropriate Handler. Pipelines supports handling with both void and results.

Generic Arguments defines result types. `Pipelines` supports handling with both void and results.

### 1.2 Handler
The epicenter of your application logic. Handlers can yield synchronous (like void or simple types) or asynchronous results.

In case, when all handlers will return exactly same return type, you don't need to define it on Input Generic Arguments. 

### 1.3 Dispatcher
Dispatcher implementation is provided by `Pipelines`. Dispatcher ensures the right Handler (with decorators) is triggered based on the Input.

NOTE:
<i>
Dispatcher is not resposible for apply Decorators by its own because Decorators gets applied on handlers during registering handlers in Dependency Injection Container.
</i>

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

There is a lot of ways how to register Open/Closed Types Decorators:

```cs
.AddDispatcher<IDispatcher>(dispatcherAssembly)
    .WithDecorator(typeof(LoggingDecorator<,>))
            .WithDecorators(x =>
            {
                x.WithImplementedInterface<IDecorator>();
                x.WithInheritedClass<BaseDecorator>();
                x.WithAttribute<DecoratorAttribute>();
                x.WithNameContaining("ExampleRequestDecoratorFourUniqueNameForSearch");
            }, decoratorsAssembly1, decoratorsAssembly2);
```

Open Type Decorator example:
```cs
public class LoggingDecorator<TInput, TResult> : IHandler<TInput, TResult>
    where TInput : IInput<TResult> where TResult : class
{
    private readonly IHandler<TInput, TResult> _handler;
   
    public LoggingDecorator(IHandler<TInput, TResult> handler)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {
        //Add logic here 
        var result = await _handler.HandleAsync(request, token);
       
        //Add logic here 
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
        //Add logic here 
        var result = await _handler.HandleAsync(request, token);

        //Add logic here 

        return result;
    }
```

#### 1.4.1 WithAttribute

When registering decorators with attribute, it can be helpful to sort decorators with attribute value. To do that, there
are `OrderBy` and `OrderByDescending` methods exposed in `WithAttribute` builder method:

```csharp
x.WithAttribute<DecoratorAttribute>().OrderBy(attr => attr.Index);

x.WithAttribute<DecoratorAttribute>().OrderByDescending(attr => attr.Index);
```

Let's check example:
```csharp
public class DecoratorAttribute : Attribute
{
    public DecoratorAttribute(int index)
    {
        Index = index;
    }

    public int Index { get; }
}
```

With this attribute you can define, which decorators should be trigger in specific order:
```csharp
[Decorator(1)]
public class
    FirstDecorator : IHandler<Input, InputResult>
{ ... }

[Decorator(2)]
public class
    SecondDecorator : IHandler<Input, InputResult>
{ ... }
```

## 2. Constraints on type parameters
Pipelines supports constraints in inputs and results but it's not mandatory. 

Examples:
```csharp
public interface IHandler<in TInput> where TInput : class, IInput { }
public interface IHandler<in TInput> where TInput : IInput { }
```
## 3. Multiple handlers for the same Input
In situations where you have multiple handlers for a single type of input, the dispatcher will execute each one, including their associated decorators. This use case is highly likely when you are creating a Pipeline for Domain Event Execution.

**Warning** - This situation is supported only, when dispatcher does not return value!

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

## 4. Execution Flow

1. An Input is dispatched using the Dispatcher.

2. The Dispatcher identifies the appropriate Handler based on the Input type.

3. If there are any Decorators, they get executed. They can apply logic before, after, or around the handler's execution.

4. The Handler executes the primary logic.

5. The result (if any) is returned to the caller.

---

## 5. Pipelines rules

### Rule 1: Input Parameter Ordering
The `Input` must be the first parameter of the Dispatcher and Handler methods.

```cs
public interface IInput<TResult> where TResult: class { } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

### Rule 2: Result Type Alignment
Result types for the `Dispatcher` and `Handler` must match. If `generic arguments` are defined in `Input`, they must also align with those in the `Dispatcher` and `Handler`.

#### Example 1: Single Result Type
```cs
public interface IInput<TResult> where TResult: class { } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

#### Example 2: Two Result Types
```cs
public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class { } 

public interface IHandler<in TInput, TResult, TResult2>
    where TInput : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    Task<(TResult, TResult2)> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(IInput<TResult, TResult2> input,
        CancellationToken token) where TResult : class where TResult2 : class;
}
```

#### Example 3: No Generic Arguments in Input
```cs
public interface IInput { }

public interface IHandler<in TInput> where TInput : IInput
{
    Task HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    Task SendAsync(IInput input, CancellationToken token);
}
```

### Rule 3: Method Parameter Matching
Method parameters for the `Dispatcher` and `Handler` must match.

#### Example: Two Parameters in the Handle Method
```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

#### Example: Four Parameters in the Handle Method
```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult : class
{
    Task<TResult> HandleAsync(TInput input, CancellationToken token, bool canDoSomething,
        Dictionary<string, string> fancyDictionary);
}

public interface IDispatcher
{
    Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token, bool canDoSomething,
        Dictionary<string, string> dictionary) where TResult : class;
}
```

### Rule 4: Non-Generic Return with Non-Generic Input
If the `Dispatcher/Handler` returns a non-generic type, then the `Input` should not have any Generic Arguments.

```cs
public interface IInput { }

public interface IHandler<in TInput> where TInput : IInput
{
    Task<string> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    Task<string> SendAsync(IInput input, CancellationToken token);
}
```

### Rule 5: Decorator Implementation
The `Decorator` must implement the Handler interface and accept a Handler as a constructor parameter.

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

public interface IInput<TResult> where TResult: class { } 

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
```

---
