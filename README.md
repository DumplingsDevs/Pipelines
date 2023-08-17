# Pipelines
------


<i>```Discover the power of the most advanced pipeline creator in the .NET ecosystem! 'Pipelines' is perfectly tailored for crafting pipelines for Queries, Commands, Domain Events, and beyond. Whether you're seeking to separate code execution logic from its invocation (Mediation pattern) our tool provides the flexibility to make your vision come to life.``` </i>

----

✅ <b>Fastest on the Market</b> - `Pipelines` guarantees rapid performance, thanks to bypassing the reflection mechanism to find the appropriate handler. Efficiency is at the core of design!

✅ <b>Unparalleled Flexibility</b> - With the capability to craft pipelines based on your own types, you have absolute control over the method inputs and the returned results 


✅ <b>Craft Multiple Pipelines</b> - The freedom to create any number of pipelines within your application, each one can be tailor-made for its specific use case

✅ <b>Minimal Dependency</b> - Developers will see the reference to 'Pipeline' namespace exclusively when they're registering with the `AddPipeline()` method. Elsewhere in the application, the presence of this library remains undetected.

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
Think of them as layers of logic that execute before or after the handler.

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
In your application's initialization, such as `Startup.cs`:

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


# Detailed documentation
------
- [Key Concepts](docs/key_concepts.md)
- [Code examples](docs/code_examples.md)
- [Configuration](docs/configuration.md)
- [Troubleshooting](docs/troubleshooting.md)
- [ADR](docs/adr.md)

# Limitations


