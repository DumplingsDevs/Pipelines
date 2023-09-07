![Pipelines](docs/assets/pipelines_purple_512w.png#gh-light-mode-only)
![Pipelines](docs/assets/pipelines_white_512w.png#gh-dark-mode-only)

[![CI-main](https://github.com/DumplingsDevs/Pipelines/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/DumplingsDevs/Pipelines/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/DumplingsDevs.Pipelines.svg)](https://www.nuget.org/packages/DumplingsDevs.Pipelines/)

------

📝 <i>``` In almost every project I've been a part of, we had a set of reusable code that was used across multiple microservices. Each time, to maintain full control over types, I had to provide my own implementation for Commands, Queries, or Domain Events. This year was no different - a new client and once again the need to implement the same solution. That's why we decided to create 'Pipelines'.```</i>

------

🛠 ```'Pipelines' is perfectly tailored for creating pipelines for Queries, Commands, Domain Events, any other pipeline utilizing your OWN types. Everywhere in your app will remain unaware of 'Pipelines' usage, with the exception of pipeline registration. If you're aiming to separate code execution logic from its invocation, such as with the Mediation pattern, this tool offers the flexibility you require.```

-----

✅ <b>Fastest on the Market</b> - `Pipelines` guarantees rapid performance, thanks to bypassing the reflection mechanism to find the appropriate handler. Efficiency is at the core of design!

✅ <b>Unparalleled Flexibility</b> - With the capability to craft pipelines based on your own types, you have absolute control over the method inputs and the returned results 

✅ <b>Craft Multiple Pipelines</b> - The freedom to create any number of pipelines within your application, each one can be tailor-made for its specific use case

✅ <b>Minimal Dependency</b> - Developers will see the reference to 'Pipeline' namespace exclusively when they're registering with the `AddPipeline()` method. Elsewhere in the application, the presence of this library remains undetected.

✅ <b>Type Validators</b> - If you inadvertently provide inconsistent types, such as discrepancies between the dispatcher and handler, type validators will alert you immediately with exceptions, ensuring the integrity and correctness of your configurations.

✅ <b>Designed with Developers in Mind</b> - Constructed with the developer's requirements at heart, our tool simplifies and accelerates your work, always upholding top-notch standards.

# 📦 Installation
----
```
dotnet add package DumplingsDevs.Pipelines
dotnet add package DumplingsDevs.Pipelines.WrapperDispatcherGenerator
```

# 🚀 Quick Start

---- 

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

# 📚 Detailed documentation
------
- [Conventions](docs/conventions.md)
- [Key Concepts](docs/key_concepts.md)
- [Benchmarks](docs/benchmarks.md)
- [Proxy vs Generated Dispatcher](docs/dispatcher_source_generator.md)
- [Code examples](docs/code_examples.md)
- [Configuration](docs/configuration.md)
- [Troubleshooting](docs/troubleshooting.md)
- [ADR](docs/adr.md)

---- 

# ⚠️ Limitations
- Pipelines in which multiple handlers will be handled for one input must have a `Task` or `void` return type.
- Cannot create a Pipeline that returns both generic and non-generic types.

# 🛤 Roadmap
- [ ] **ADR Documentation**: Record key architectural decisions made during implementation using ADRs.
- [ ] **Code Cleanup**: Refine and tidy up the codebase post-MVP, paving the way for new feature development.
- [ ] **Support for Nullable Results**: Add functionality to handle nullable result types.
- [ ] **Configurable Dispatcher Behavior**: Implement configuration settings to decide if the dispatcher should throw an exception in case there's no handler for a given Input.
- [ ] **Dependency Injection Scope Choice**: Provide an option to decide whether or not to create a Dependency Injection Scope in Dispatchers.
- [ ] **Multiple Inputs in Dispatcher**: Enhance the dispatcher handle method to accept a list of inputs instead of just one.
- [ ] **Parallel Pipeline**: Introduce a pipeline to facilitate parallel execution of multiple handlers.
- [ ] **Stream Pipeline**: Implement support for streaming pipelines.
- [ ] **Decorator Performance Optimization**: Improve performance, especially concerning the use of `ActivatorUtilities.CreateInstance()`.

# 🥟💡 The Dumplings Behind the Magic
Hey there! We're Dumplings Devs, made up of <a href="https://pl.linkedin.com/in/matwroblewski">Mateusz Wróblewski</a> and <a href="https://pl.linkedin.com/in/kamil-bytner">Kamil Bytner</a>. We're passionate about software and always up for a coding challenge. 



