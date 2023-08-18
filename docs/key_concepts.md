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
Analogous to Middlewares in .NET. Decorators wrap around handlers to extend or modify their behavior. Think of them as layers of logic that execute before or after the handler. Decorators can be applied both for Open Types and Closed Types.

To implement new decorator there two things that needs to be done:
- implement Handler interface
- inject Handler instance in Constructor

When registering decorators, ensure the order of registration in the DI container is the same as the execution order you desire.

//              │   ┌─────────────────────┐   │
//              │   │ Decorator1:IHandler │   │
//              │   └─────────────────────┘   │
//              │                             │
//              │   ┌─────────────────────┐   │
//              │   │ Decorator2:IHandler │   │
//              │   └─────────────────────┘   │
// Registration │                             │  Invoke
//  direction   │   ┌─────────────────────┐   │ direction
//              │   │ Decorator3:IHandler │   │
//              │   └─────────────────────┘   │
//              ▼                             │
//                   ┌──────────────────┐     │
//                   │ Handler:IHandler │     │
//                   └──────────────────┘     │
//                                            │
//                  ┌─────────────────────┐   │
//                  │ Decorator3:IHandler │   │
//                  └─────────────────────┘   │
//                                            │
//                  ┌─────────────────────┐   │
//                  │ Decorator2:IHandler │   │
//                  └─────────────────────┘   │
//                                            │  
//                  ┌─────────────────────┐   │ 
//                  │ Decorator1:IHandler │   │
//                  └─────────────────────┘   │
//                                            ▼


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

## Execution Flow

1. An Input is dispatched using the Dispatcher.

2. The Dispatcher identifies the appropriate Handler based on the Input type.

3. If there are any Decorators, they get executed. They can apply logic before, after, or around the handler's execution.

4. The Handler executes the primary logic.

5. The result (if any) is returned to the caller.