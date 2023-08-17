# Pipelines
------


<i>```Discover the power of the most advanced pipeline creator in the .NET ecosystem! Our library is perfectly tailored for crafting pipelines for Queries, Commands, Domain Events, and beyond. Whether you're seeking to separate code execution logic from its invocation (Mediation pattern) our tool provides the flexibility to make your vision come to life.``` </i>

----

✅ <b>Fastest on the Market</b> - `Pipelines` guarantees rapid performance, thanks to bypassing the reflection mechanism to find the appropriate handler. Efficiency is at the core of our design!

✅ <b>Unparalleled Flexibility</b> - With the capability to craft pipelines based on your own types, you have absolute control over the method inputs and the returned results 


✅ <b>Craft Multiple Pipelines</b> - The freedom to create any number of pipelines within your application, each one can be tailor-made for its specific use case

✅ <b>Minimal Dependency</b> - Developers will see the reference to 'Pipeline' namespace exclusively when they're registering with the AddPipeline() method. Elsewhere in the application, the presence of this library remains undetected.

✅ <b>Type Validators</b> - If you inadvertently provide inconsistent types, such as discrepancies between the dispatcher and handler, type validators will alert you immediately with exceptions, ensuring the integrity and correctness of your configurations.

✅ <b>Designed with Developers in Mind</b> - Constructed with the developer's requirements at heart, our tool simplifies and accelerates your work, always upholding top-notch standards.

# Installation
----
```
dotnet add package DumplingsDevs.Pipelines
dotnet add package DumplingsDevs.Pipelines.Generators
```

# Quick Start
---- 
## 1. Define your own types

### 1.1 Input 

The "Input" acts as the initial parameter for Handler and Dispatcher methods, guiding the search for the relevant Handler.

```cs
public interface IInput<TResult>{ }
```

### 1.2 Handler
Handlers house the application logic and can generate both synchronous and asynchronous results.

```cs
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
```

### 1.3 Dispatcher
Serving as a bridge between inputs and their respective handlers, the Dispatcher ensures the appropriate Handler is triggered for a given Input.

```cs
public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}
```


## 2. Implement first decorator (optional step)
```cs
public class LoggingDecorator<TCommand, TResult> : IHandler<TCommand, TResult> where TCommand : IInput<TResult>
{
    private readonly IHandler<TCommand, TResult> _handler;
    private readonly ILogger _logger;
    
    public LoggingDecorator(IHandler<TCommand, TResult> handler, ILogger logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
    {
        _logger.Log(LogLevel.Information,"Executing handler for input {0}", typeof(TCommand));
        var result = await _handler.HandleAsync(request, token);
        _logger.Log(LogLevel.Information,"Executed handler for input {0}", typeof(TCommand));

        return result;
    }
}
```

## 3. Implement first handler

### 3.1 Input and Result

```cs
public record ExampleInput(string Value) : IInput<ExampleCommandResult>;

public record ExampleCommandResult(string Value);
```

### 3.2 Handler 
```cs
public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
```
## 4. Register pipeline
In your application's initialization, such as Startup.cs:

```cs
var handlersAssembly = //Assembly where handlers assembly are implemented
var dispatcherAssembly = //Assembly where AddPipeline gets invoked

_services
    .AddPipeline()
    .AddInput(typeof(IInput<>))
            .AddHandler(typeof(IHandler<,>), handlersAssembly)
            .AddDispatcher<IDispatcher>(dispatcherAssembly)
              .WithOpenTypeDecorator(typeof(LoggingDecorator<,>));

```

## 5. Example Usege (Fluent API .NET)

```cs
public static void CreateExampleEndpoint(this WebApplication app)
    {
        app.MapPost("/example", async (ExampleInput request, IDispatcher dispatcher, CancellationToken token) =>
        {
            var result = await dispatcher.SendAsync(command,token);

            return Results.Ok();
        });
    }
```


# Key Concepts
------
## Building blocks
### 1. Input 
First method parameter in the Handler and Dispatcher methods, guiding the identification of the appropriate Handler. Pipelines supports handling with both void and results.

Generic Arguments defines result types. `Pipelines` supports handling with both void and results.

Examples: 
```cs
public interface ICommand {} 
public interface ICommand<TResult>{}
public interface ICommand<TResult,TResult2> {}
```

### 2. Handler
The epicenter of your application logic. Handlers can yield synchronous (like void or simple types) or asynchronous results.

In case, when all handlers will return exactly same return type, you don't need to define it on Input Generic Arguments. 

Examples: 
```cs
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public void Handle(TCommand command, CancellationToken token);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public string Handle(TCommand command, CancellationToken token);
}

public interface ICommandHandler<in TCommand, TResult, TResult2> where TCommand : ICommand<TResult, TResult2>
{
    public (TResult, TResult2) Handle(TCommand command, CancellationToken token);
}
```

```cs
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}

public interface ICommandHandler<in TCommand, TResult, TResult2> where TCommand : ICommand<TResult, TResult2>
{
    public Task<(TResult, TResult2)> HandleAsync(TCommand command, CancellationToken token);
}
```

### 3. Dispatcher
Dispatcher implementation is provided by `Pipelines`. Dispatcher ensures the right Handler (with decorators) is triggered based on the Input.

NOTE:
<i>
Dispatcher is not resposible for apply Decorators by its own because Decorators gets applied on handlers during registering handlers in Dependency Injection Container.
</i>

Examples:

```cs
public interface ICommandDispatcher
{
    public void Send(ICommand command);
}

public interface ICommandDispatcher
{
    public string Send(ICommand command);
}

public interface ICommandDispatcher
{
    public (TResult, TResult2) Send<TResult, TResult2>(ICommand<TResult, TResult2> command);
}
```

```cs
public interface ICommandDispatcher
{
    public Task SendAsync(ICommand command, CancellationToken token);
}

public interface ICommandDispatcher
{
    public Task<string> SendAsync(ICommand command, CancellationToken token);
}

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token);
}

public interface ICommandDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult, TResult2>(ICommand<TResult, TResult2> command,
        CancellationToken token);
}
```

### 4. Decorators
Analogous to Middlewares in .NET. Decorators wrap around handlers to extend or modify their behavior. Think of them as layers of logic that execute before or after the handler. Decorators can be applied both for OpenTypes and ClosedTypes.

To implement new decorator there two things that needs to be done:
- implement Handler interface
- inject Handler instance in Constructor

When registering decorators, ensure the order of registration in the DI container is the same as the execution order you desire.

There is a lot of ways how to register Closed Types Decorators:

```cs
.AddDispatcher<ICommandDispatcher>(dispatcherAssembly)
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
public class LoggingDecorator<TCommand, TResult> : IHandler<TCommand, TResult> where TCommand : IInput<TResult>
{
    private readonly IHandler<TCommand, TResult> _handler;
   
    public LoggingDecorator(IHandler<TCommand, TResult> handler)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TCommand request, CancellationToken token)
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

# Execution Flow

1. An Input is dispatched using the Dispatcher.

2. The Dispatcher identifies the appropriate Handler based on the Input type.

3. If there are any Decorators, they get executed. They can apply logic before, after, or around the handler's execution.

4. The Handler executes the primary logic.

5. The result (if any) is returned to the caller.

# Configuration options

# Limitations


