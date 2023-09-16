# Configuration

In this section of the documentation, you'll learn how to configure Pipeline.

-----

## Table of Contents

- [1. Pipeline Builder](#1-pipeline-builder)
  - [1.1 DispatcherOptions](#11-dispatcheroptions)
  - [1.2 DecoratorOptions](#12-decoratoroptions)
-----

## 1. Pipeline Builder

### 1.1 DispatcherOptions

Dispatcher options can be set using the `AddDispatcher<>()` method.

#### 1.1.1 UseReflectionProxyImplementation

**Default value**: `false`

Choose the type of Dispatcher your application would like to use:

- **When true**: The Dispatcher uses an implementation based on reflection.
- **When false**: The Dispatcher uses a source-generated dispatcher.

#### 1.1.2 CreateDIScope

**Default value**: `true`

Determines whether to create a new dependency injection scope when dispatching inputs.

- **When true**: The Dispatcher creates a new Dependency Injection scope during processing.
- **When false**: The Dispatcher uses the existing scope.

#### 1.1.3 ThrowExceptionIfHandlerNotFound

**Default value**: `true`

Determines whether to throw a `HandlerNotRegisteredException` if no handler is found for the given input.
**NOTE**: This configuration only applies to Pipelines that don't return results (i.e., `void` or `Task`).

- **When true**: If no handler is found, the Dispatcher will throw a `HandlerNotRegisteredException`.
- **When false**: The Dispatcher will proceed without throwing an exception if no handler is found.

### 1.2 DecoratorOptions

Decorator options can be set using the `WithDecorator()` and `WithDecorators()` methods.

#### 1.2.1 StrictMode

**Default value**: `true`

Choose whether Validators should throw an exception when the Pipeline Builder detects an incorrectly implemented decorator:

- **When true**: If the Pipeline Builder detects an incorrect Decorator implementation, it will throw an exception.
- **When false**: If the Pipeline Builder detects an incorrect Decorator implementation, it will skip this decorator. The decorator will not be applied to handlers, and no exception will be thrown.