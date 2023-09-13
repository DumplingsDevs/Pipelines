# 🚀 Quick Start

In this section of the documentation, you'll learn how to build first Pipeline.

------


## 1️⃣ Define your own types

### 1.1 Input 

The "Input" acts as the initial parameter for Handler and Dispatcher methods, guiding the search for the relevant Handler.

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public interface IInput<TResult> where TResult: class{ } 
```

</details>

### 1.2 Handler

Handlers house the application logic and can generate both synchronous and asynchronous results.

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
```

</details>

### 1.3 Dispatcher

Serving as a bridge between inputs and their respective handlers, the Dispatcher ensures the appropriate Handler is triggered for a given Input.

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}
```

</details>

## 2️⃣ Implement first decorator (optional step)

Analogous to Middlewares in .NET. Think of them as layers of logic that execute before or after the handler.

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public class LoggingDecorator<TInput, TResult> : IHandler<TInput, TResult> where TInput : IInput<TResult> where TResult : class
{
    private readonly IHandler<TInput, TResult> _handler;
    private readonly ILogger _logger;
    
    public LoggingDecorator(IHandler<TInput, TResult> handler, ILogger logger)
    {
        _handler = handler;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(TInput request, CancellationToken token)
    {
        _logger.Log(LogLevel.Information,"Executing handler for input {0}", typeof(TInput));
        var result = await _handler.HandleAsync(request, token);
        _logger.Log(LogLevel.Information,"Executed handler for input {0}", typeof(TInput));

        return result;
    }
}
```

</details>

## 3️⃣  Implement first handler

### 3.1 Input and Result

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public record ExampleInput(string Value) : IInput<ExampleCommandResult>;
public record ExampleCommandResult(string Value);
```

</details>

### 3.2 Handler 

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
```

</details>

## 4️⃣ Register pipeline

In your application's initialization, such as `Startup.cs`:

<b> IMPORTANT! All provided types must be specified using the typeof() method! </b>

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
var handlersAssembly = //Assembly where handlers assembly are implemented
var dispatcherAssembly = //Assembly where AddPipeline gets invoked

_services
    .AddPipeline()
    .AddInput(typeof(IInput<>))
            .AddHandler(typeof(IHandler<,>), handlersAssembly)
            .AddDispatcher<IDispatcher>(dispatcherAssembly)
              .WithDecorator(typeof(LoggingDecorator<,>));
```

</details>

## 5️⃣ Example Usage (Fluent API .NET)

<details>
<summary style="color: green">📜 Show me code </summary>

```cs
public static void CreateExampleEndpoint(this WebApplication app)
    {
        app.MapPost("/example", async (ExampleInput request, IDispatcher dispatcher, CancellationToken token) =>
        {
            var result = await dispatcher.SendAsync(input,token);
            return Results.Ok();
        });
    }
```

</details>

---- 