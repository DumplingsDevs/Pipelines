# Troubleshooting
------

In this section, you'll find descriptions of exceptions that may arise while using 'Pipelines' and guidance on how to debug the source generated dispatcher.

------
## Table of Content 

- [1. Pipeline Builder Exceptions](#1-pipeline-builder-exceptions)
  - [ProvidedTypeIsNotInterfaceException](#providedtypeisnotinterfaceexception)
  - [HandlerMethodNotFoundException](#handlermethodnotfoundexception)
  - [MultipleHandlerMethodsException](#multiplehandlermethodsexception)
  - [MethodShouldHaveAtLeastOneParameterException](#methodshouldhaveatleastoneparameterexception)
  - [GenericArgumentsLengthMismatchException](#genericargumentslengthmismatchexception)
  - [GenericArgumentsNotFoundException](#genericargumentsnotfoundexception)
  - [HandlerInputTypeMismatchException](#handlerinputtypemismatchexception)
  - [InvalidConstraintLengthException](#invalidconstraintlengthexception)
  - [ExpectedMethodWithResultException](#expectedmethodwithresultexception)
  - [ExpectedVoidMethodException](#expectedvoidmethodexception)
  - [ResultTypeCountMismatchException](#resulttypecountmismatchexception)
  - [GenericTypeCountMismatchException](#generictypecountmismatchexception)
  - [IsGenericMismatchException](#isgenericmismatchexception)
  - [TypeMismatchException](#typemismatchexception)
  - [DispatcherMethodInputTypeMismatchException](#dispatchermethodinputtypemismatchexception)
  - [ParameterCountMismatchException](#parametercountmismatchexception)
  - [ParameterTypeMismatchException](#parametertypemismatchexception)
  - [TaskReturnTypeMismatchException](#taskreturntypemismatchexception)
  - [VoidAndValueMethodMismatchException](#voidandvaluemethodmismatchexception)
  - [InterfaceImplementationException](#interfaceimplementationexception)
  - [ConstructorValidationException](#constructorvalidationexception)

- [2. Runtime Exceptions](#2-runtime-exceptions)
  - [HandlerNotRegisteredException](#handlernotregisteredexception)
  - [InputNotSupportedByDispatcherException](#inputnotsupportedbydispatcherexception)
  - [InputNullReferenceException](#inputnullreferenceexception)
   
- [3. How to debug Source Generated Dispatcher](#3-how-to-debug-source-generated-dispatcher)

------

## 1. Pipeline Builder Exceptions

### ProvidedTypeIsNotInterfaceException

#### What happened?
One of the types provided to the pipeline is not an interface.

#### Bad example

```csharp
public class SampleClass{}

services
    .AddPipeline()
    .AddInput(typeof(SampleClass))
            .AddHandler(typeof(IHandler<>), handlersAssembly)
            .AddDispatcher<IDispatcher>(dispatcherAssembly)
...
```

#### How to fix
1. Ensure all provided types are interfaces.
2. Always use typeof when providing types.

```cs
public interface IInput{}

services
    .AddPipeline()
    .AddInput(typeof(IInput))
            .AddHandler(typeof(IHandler<>), handlersAssembly)
            .AddDispatcher<IDispatcher>(dispatcherAssembly)
...
```
---

### HandlerMethodNotFoundException

#### What happened?
A provided type doesn't define a handle method.

#### Bad example

```csharp

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{}

...
```

#### How to fix
Define a Handle method in the handler.

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
``` 
...

---

### MultipleHandlerMethodsException

#### What happened?
Multiple methods were defined in the provided type. Each interface must contain only a single method.

#### Bad example

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
    public Task<TResult> HandleAsync(TInput input);
}
```

#### How to fix
Remove extra methods from the interface.

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
```
---

### MethodShouldHaveAtLeastOneParameterException

#### What happened?
The defined Handle method does not have any parameters.

#### Bad example

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync();
}
```

#### How to fix
Ensure the method has at least one parameter, which should be of the Input Type.

```cs
public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
```

---

### GenericArgumentsNotFoundException

#### What happened?

The `Handler` lacks generic arguments. At least one generic argument is required, specifically for `Input`.

#### Bad example

```csharp
public interface IHandler
{
    Task HandleAsync(IInput input, CancellationToken token);
}
```

#### How to fix
Ensure the `Handler` has at least one generic argument representing the `Input`.

```csharp
public interface IHandler<in TInput> where TInput : IInput
{
    Task HandleAsync(TInput input, CancellationToken token);
}
```
---

### HandlerInputTypeMismatchException

#### What happened?

The first generic argument doesn't have the `Input` type as its constraint.

#### Bad example

```csharp
public interface IInput
{ }

public interface IHandlerWithResult<in TInput, TResult> 
    where TInput : IInputWithResult<TResult> 
    where TResult : class
{
    Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
```

#### How to fix
The first generic argument of the handler must use the `Input` type, specified in the `AddInput()` method, as its constraint.

```csharp
public interface IInput
{ }

public interface IHandler<in TInput> where TInput : IInput
{
    Task HandleAsync(TInput input, CancellationToken token);
}
```

---

### InvalidConstraintLengthException

#### What happened?

The first generic argument lacks constraints. At least one constraint is required, specifically for the Input type.

#### Bad example

```cs
public interface IInput
{ }

public interface IHandler<in TInput>
{
    public Task HandleAsync(TInput input, CancellationToken token);
}
```

#### How to fix
Ensure the first generic argument of the handler has the Input type as its constraint.

```cs
public interface IInput
{ }

public interface IHandler<in TInput> where TInput : IInput
{
    Task HandleAsync(TInput input, CancellationToken token);
}
```
---

### ExpectedMethodWithResultException

#### What happened?

Given the Input generic arguments, it was anticipated that the Handler/Dispatcher would have a method returning a result, but a void was found instead.

#### Bad example

```cs
public interface IInput<TResult>
{}

public interface IDispatcher
{
    public Task SendAsync<TResult>(IInput<TResult> request, CancellationToken token);
}

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult>
{
    public Task HandleAsync(TInput command, CancellationToken token);
}

```

#### How to fix
Ensure that result types align with the Input generic arguments.

```cs
public interface IInput<TResult>
{}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> request, CancellationToken token);
}

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
```

---

### ResultTypeCountMismatchException

#### What happened?

The number of result types defined by the Input generic arguments does not match the number of result types found in the Dispatcher/Handler method.

#### Bad example

```cs
public interface IInputWithResult<TResult>
{ }


public interface IHandler<in TInput, TResult>
    where TInput : IInputWithResult<TResult> where TResult : class
{
    public Task<(TResult, bool)> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<(TResult, TResult2)> SendAsync<TResult,TResult2>(IInputWithResult<TResult> request, CancellationToken token);
}

```

#### How to fix
Ensure that the Dispatcher/Handler method's result types align with the Input generic arguments.

```cs
public interface IInputWithResult<TResult>
{ }


public interface IHandler<in TInput, TResult>
    where TInput : IInputWithResult<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInputWithResult<TResult> request, CancellationToken token);
}

```
---

### GenericTypeCountMismatchException

#### What happened?

The number of constraints in the result types defined in the Handler or Dispatcher does not match.

#### Bad example

```cs
public interface IInputType
{}

public interface IHandler<in TInput, TResultOne, TResultTwo> where TInput : IInputType
    where TResultOne : IResultOne
    where TResultTwo : IResultTwo
{
    public Task<(TResultOne,TResultTwo)> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType)
        where TResult : IResultOne;
}
```

#### How to fix
Ensure that the number of constraints on result types in the Handler or Dispatcher matches.

```cs
public interface IInputType
{}

public interface IHandler<in TInput, TResultOne, TResultTwo> where TInput : IInputType
    where TResultOne : IResultOne
    where TResultTwo : IResultTwo
{
    public Task<(TResultOne,TResultTwo)> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<(TResult, TResultTwo)> SendAsync<TResult, TResultTwo>(IInputType inputType)
        where TResult : IResultOne
        where TResultTwo : IResultTwo;
}

```
---

### IsGenericMismatchException

#### What happened?

There's a mismatch between the result types in the Handler and Dispatcher: one is generic, while the other is not.

#### Bad example

```cs
public interface IHandler<in TInput, TResult> where TInput : IInputType
    where TResult : IResult
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}
```

#### How to fix
Ensure that both the Handler and Dispatcher return the same type of result.

```cs
public interface IHandler<in TInput, TResult> where TInput : IInputType
    where TResult : IResult
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInputType inputType, CancellationToken token);
}

```
---

### TypeMismatchException

There's a mismatch between the non generic result types in the Handler and Dispatcher.

#### What happened?

#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public int HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public string SendAsync(IInputType input);
}
```

#### How to fix

Ensure that both the Handler and Dispatcher return the same type of result.

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public int HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public int SendAsync(IInputType input);
}
```
---

### DispatcherMethodInputTypeMismatchException

#### What happened?

The Dispatcher's Handle method does not use the expected Input type as its first parameter.

#### Bad example

```cs
public interface IInput
{ }

public interface IDispatcher
{
    public Task SendAsync(int request, CancellationToken token);
}
```

#### How to fix
Ensure that the Input type is used as the first parameter in the method.

```cs
public interface IInput
{ }

public interface IDispatcher
{
    public Task SendAsync(IInput request, CancellationToken token);
}
```
---

### ParameterCountMismatchException

#### What happened?

The Handler and Dispatcher methods have a mismatch in the number of parameters.

#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, int index, CancellationToken token);
}
```

#### How to fix

Ensure that the Handler and Dispatcher methods have the same number and type of parameters.

```cs

public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput input, int index, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, int index, CancellationToken token);
}

```
---

### ParameterTypeMismatchException

#### What happened?

There's a mismatch in the type of a parameter between the Handler and Dispatcher methods.


#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, int index, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, string text, CancellationToken token);
}

```

#### How to fix

Ensure that the parameters in both the Handler and Dispatcher methods are of the same type and order.

```cs

public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, string text, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, string text, CancellationToken token);
}

```
---

### TaskReturnTypeMismatchException

#### What happened?

There's a mismatch in the return type of the Handler and Dispatcher methods. One method returns a Task<> type, while the other does not.

#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public string HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}
```

#### How to fix
Ensure that both the Handler and Dispatcher methods have consistent return types. If one returns a Task<>, the other should too.

```cs

public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}
```
---

### VoidAndValueMethodMismatchException

#### What happened?

There's an inconsistency in the return types between the Handler and Dispatcher methods. One method returns a value, while the other returns void.

#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public string HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcherVoid
{
    public void SendAsync(IInputType inputType);
}
```

#### How to fix

Ensure both the Handler and Dispatcher methods have matching return types. Either both should return a value, or both should return void.

```cs
public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput input, CancellationToken token);
}

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType);
}

```
---

### InterfaceImplementationException

#### What happened?

The decorator class is not implementing the expected interface, leading to a type mismatch between the expected and actual generic types.

#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput input, CancellationToken token);
}

public class Decorator : IDifferentHandler<InputWithResult, Result>
{
    private readonly IDifferentHandler<InputWithResult, Result> _handler;

    public Decorator(IDifferentHandler<InputWithResult, Result> handler)
    {
        _handler = handler;
    }

    public async Task<Result> HandleAsync(InputWithResult input, CancellationToken token)
    {
        return await _handler.HandleAsync(input, token);
    }
}

```

#### How to fix

Ensure that the decorator class implements the correct interface, which it's intended to decorate. The generic type parameters of the decorator should align with those of the interface it should implement.


```cs
public class Decorator : IHandler<IInput, Result>
{
    private readonly IHandler<IInput, Result> _handler;

    public Decorator(IHandler<IInput, Result> handler)
    {
        _handler = handler;
    }

    public async Task<Result> HandleAsync(IInput input, CancellationToken token)
    {
        return await _handler.HandleAsync(input, token);
    }
}
```
---

### ConstructorValidationException

#### What happened?

The decorator's constructor does not have the required handler dependency (either it's missing or invalid).

#### Bad example

```cs
public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput input, CancellationToken token);
}

public class Decorator : IHandler<IInput, Result>
{
    public Decorator()
    { }

    public async Task<Result> HandleAsync(IInput input, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}

```

#### How to fix

Include the required handler dependency in the decorator's constructor and use it in the HandleAsync method:

```cs

public class Decorator : IHandler<IInput, Result>
{
    private readonly IHandler<IInput, Result> _handler;

    public Decorator(IHandler<IInput, Result> handler)
    {
        _handler = handler;
    }

    public async Task<Result> HandleAsync(IInput input, CancellationToken token)
    {
        return await _handler.HandleAsync(input, token);
    }
}

```
---

## 2. Runtime Exceptions

### DispatcherNotRegisteredException

#### What happened?
The dispatcher registration is missing from the Dependency Injection Container. By default, the `AddDispatcher()` method use the Dispatcher Generated by Source generator. The primary reason the Generated Dispatcher isn't found is due to the absence of package in project `Pipelines.WrapperDispatcherGenerator`.

#### How to fix
Add package `DumplingsDevs.Pipelines.WrapperDispatcherGenerator` or change Dispatcher options to `UseReflectionProxyImplementation=true`

```
dotnet add package DumplingsDevs.Pipelines.WrapperDispatcherGenerator
```

or

```cs
services
    .AddPipeline()
    .AddInput(typeof(ICommand<>))
    .AddHandler(typeof(ICommandHandler<,>), assembly)
    .AddDispatcher<ICommandDispatcher>(new DispatcherOptions(true)
    .Build()
```

---

### HandlerNotRegisteredException

#### What happened?

The handler implementation for the provided input was not found. In most cases, this suggests that while the input has been defined, its corresponding handler implementation is missing.

...

#### Bad example

```csharp
public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;
...

var request = new ExampleCommand2("My test request");
var result = await _dispatcher.SendAsync(request, new CancellationToken());

```

#### How to fix

For every defined Input, ensure that a corresponding Handler is implemented.


```csharp
public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;

public class ExampleHandler : IHandler<ExampleCommand2, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleCommand2 input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
...

var request = new ExampleCommand2("My test request");
var result = await _dispatcher.SendAsync(request, new CancellationToken());

```

---

### InputNotSupportedByDispatcherException

#### What happened?

...

#### Bad example

```csharp
...
```

#### How to fix

...

---

### InputNullReferenceException

#### What happened?

The dispatcher was invoked with a null value instead of an actual input object.


#### Bad example

```csharp
var result = await _dispatcher.SendAsync(null, new CancellationToken());
```

#### How to fix
Ensure that you provide a valid input object to the method and avoid passing null values.

```csharp
var request = new ExampleCommand2("My test request");
var result = await _dispatcher.SendAsync(request, new CancellationToken());
```

---

## 3. How to debug Source Generated Dispatcher
