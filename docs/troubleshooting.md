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
  - [ConstructorValidationException](#constructorvalidationexception)
  - [InterfaceImplementationException](#interfaceimplementationexception)

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

### GenericArgumentsLengthMismatchException

#### What happened?

The number of generic arguments for `Input` defined in `AddInput()` does not align with the `Input` specified in the Handler definition.

It's likely that a different `Input` type was used in the `AddInput()` method compared to the one in the `Handler` type definition.

#### Bad example

```csharp
public interface IInput
{ }

public interface IHandlerWithResult<in TInput, TResult> 
    where TInput : IInputWithResult<TResult> 
    where TResult : class
{
    Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
```

#### How to fix
Ensure the same `Input` type is used in both the `AddInput()` method and the `Handler` definition.

```csharp
public interface IInputWithResult<TResult> where TResult : class
{ }

public interface IHandlerWithResult<in TInput, TResult> 
    where TInput : IInputWithResult<TResult> 
    where TResult : class
{
    Task<TResult> HandleAsync(TInput command, CancellationToken token);
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
    Task HandleAsync(IInput command, CancellationToken token);
}
```

#### How to fix
Ensure the `Handler` has at least one generic argument representing the `Input`.

```csharp
public interface IHandler<in TInput> where TInput : IInput
{
    Task HandleAsync(TInput command, CancellationToken token);
}
```
---

### HandlerInputTypeMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### InvalidConstraintLengthException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### ExpectedMethodWithResultException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### ExpectedVoidMethodException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### ResultTypeCountMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### GenericTypeCountMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### IsGenericMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### TypeMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### DispatcherMethodInputTypeMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### ParameterCountMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### ParameterTypeMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### TaskReturnTypeMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### VoidAndValueMethodMismatchException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### ConstructorValidationException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

### InterfaceImplementationException

#### What happened?

#### Bad example

```cs
```

#### How to fix
```cs
```
---

## 2. Runtime Exceptions

### HandlerNotRegisteredException

#### What happened?

...

#### Bad example

```csharp
...
```

#### How to fix

...

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

...

#### Bad example

```csharp
...
```

#### How to fix

...

---

## 3. How to debug Source Generated Dispatcher
