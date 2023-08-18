# Troubleshooting
------

In this section, you'll find descriptions of exceptions that may arise while using 'Pipelines' and guidance on how to debug the source generated dispatcher.

------
## Table of Content 

- [1. Pipeline Builder Exceptions](#1-pipeline-builder-exceptions)
  - [1.1 ProvidedTypeIsNotInterfaceException](#11-providedtypeisnotinterfaceexception)
  - [1.2 HandlerMethodNotFoundException](#12-handlermethodnotfoundexception)
  - [1.3 MultipleHandlerMethodsException](#13-multiplehandlermethodsexception)
  - [1.4 MethodShouldHaveAtLeastOneParameterException](#14-methodshouldhaveatleastoneparameterexception)
  - [1.5 GenericArgumentsLengthMismatchException](#15-genericargumentslengthmismatchexception)
  - [1.6 GenericArgumentsNotFoundException](#16-genericargumentsnotfoundexception)
  - [1.7 HandlerInputTypeMismatchException](#17-handlerinputtypemismatchexception)
  - [1.8 InvalidConstraintLengthException](#18-invalidconstraintlengthexception)
  - [1.9 ExpectedMethodWithResultException](#19-expectedmethodwithresultexception)
  - [1.10 ExpectedVoidMethodException](#110-expectedvoidmethodexception)
  - [1.11 ResultTypeCountMismatchException](#111-resulttypecountmismatchexception)
  - [1.12 GenericTypeCountMismatchException](#112-generictypecountmismatchexception)
  - [1.13 IsGenericMismatchException](#113-isgenericmismatchexception)
  - [1.14 TypeMismatchException](#114-typemismatchexception)
  - [1.15 DispatcherMethodInputTypeMismatchException](#115-dispatchermethodinputtypemismatchexception)
  - [1.16 ParameterCountMismatchException](#116-parametercountmismatchexception)
  - [1.17 ParameterTypeMismatchException](#117-parametertypemismatchexception)
  - [1.18 ResultTypeCountMismatchException](#118-resulttypecountmismatchexception)
  - [1.19 TaskReturnTypeMismatchException](#119-taskreturntypemismatchexception)
  - [1.20 VoidAndValueMethodMismatchException](#120-voidandvaluemethodmismatchexception)
  - [1.21 ConstructorValidationException](#121-constructorvalidationexception)
  - [1.22 InterfaceImplementationException](#122-interfaceimplementationexception)

- [2. Runtime Exceptions](#2-runtime-exceptions)
  - [2.1 HandlerNotRegisteredException](#21-handlernotregisteredexception)
  - [2.2 InputNotSupportedByDispatcherException](#22-inputnotsupportedbydispatcherexception)
  - [2.3 InputNullReferenceException](#23-inputnullreferenceexception)
   
- [3. How to debug Source Generated Dispatcher](#3-how-to-debug-source-generated-dispatcher)

------

