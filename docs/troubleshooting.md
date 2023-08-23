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
  - [ResultTypeCountMismatchException](#resulttypecountmismatchexception)
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

public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult: class
{}

...
```

#### How to fix
Define a Handle method in the handler.

```cs
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
``` 
...

---

### MultipleHandlerMethodsException

#### What happened?
Multiple methods were defined in the provided type. Each interface must contain only a single method.

#### Bad example

```cs
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
    public Task<TResult> HandleAsync(TCommand command);
}
```

#### How to fix
Remove extra methods from the interface.

```cs
public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
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
