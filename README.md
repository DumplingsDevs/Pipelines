[![CI-main](https://github.com/DumplingsDevs/Pipelines/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/DumplingsDevs/Pipelines/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/DumplingsDevs.Pipelines.svg)](https://www.nuget.org/packages/DumplingsDevs.Pipelines/)

-----

<p align="center">
  <img src="docs/assets/pipelines_purple.svg#gh-light-mode-only" alt="Pipelines"/>
  <img src="docs/assets/pipelines_white.svg#gh-dark-mode-only" alt="Pipelines"/>
</p>



🛠 ```We belief that a good library should adapt to your application, rather than forcing your application to adapt to it. We recognize that every software project is unique and may employ different architectural patterns and designs. With Pipelines, we found a library that enables us to build our applications around our preferred patterns, rather than constraining us to rigid structures imposed by external libraries. This flexibility not only streamlines our development process but also empowers us to make design choices that best suit our specific use cases.```

```Pipelines empowers developers to leverage the potential of the Mediator pattern seamlessly, thanks to its flexible and adaptable foundations.```

-----

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

---

<p align="center">
  <img src="docs/assets/dumplings_purple.svg#gh-light-mode-only" alt="DumplingsDevs"/>
  <img src="docs/assets/dumplings_white.svg#gh-dark-mode-only" alt="DumplingsDevs"/>
</p>
