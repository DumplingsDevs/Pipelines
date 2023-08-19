# Configuration
------

In this section of the documentation, you'll learn how you can configure Pipeline.

-----

## Table of Content 

- [1. Pipeline Builder](#1-pipeline-builder)
  - [1.1 DispatcherOptions](#11-dispatcheroptions)
  - [1.2 DecoratorOptions](#12-decoratoroptions)

------

## 1. Pipleline builder

### 1.1 DispatcherOptions

Dispatcher options can be set in AddDispatcher<>() method.

### 1.1.1 UseReflectionProxyImplementation

<b>Default value</b> - false

Allow to chose which type of Dispatcher application would like to use.

When true, the Dispatcher will use implementation based on Reflection.

When false, the Dispatcher will use source generated dispatcher.

### 1.2 DecoratorOptions

Decorator options can be set in WithOpenTypeDecorator()/WithClosedTypeDecorator() methods.

### 1.2.1 StrictMode
<b>Default value</b> - true

Allow to chose if Validators should throw an exception when Pipeline builder will found out that decorators is not implemented correctly.

When true, when Pipeline Builder will found out incorrect Decorator implementation, will throw an exception.

When false, when Pipeline Builder will found out incorrect Decorator implementaiton, it will skip this decorator and it will be not applied on handlers, without throwing an exception.


