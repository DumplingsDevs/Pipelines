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
