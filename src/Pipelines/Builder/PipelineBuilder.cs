using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Decorators;
using Pipelines.Builder.Exceptions;
using Pipelines.Builder.HandlerWrappers;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Options;
using Pipelines.Builder.Validators.CrossValidation.MethodParameters;
using Pipelines.Builder.Validators.CrossValidation.ResultType;
using Pipelines.Builder.Validators.Decorator;
using Pipelines.Builder.Validators.Dispatcher.InputType;
using Pipelines.Builder.Validators.Dispatcher.ResultTypes;
using Pipelines.Builder.Validators.Handler.InputType;
using Pipelines.Builder.Validators.Handler.ResultTypes;
using Pipelines.Builder.Validators.Shared.InterfaceConstraint;
using Pipelines.Builder.Validators.Shared.MethodWithOneParameter;
using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;
using Pipelines.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder;

internal class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerInterfaceType = null!;
    private MethodInfo _handlerHandleMethod = null!;
    private Assembly[] _handlerAssemblies = null!;
    private Type _inputInterfaceType = null!;
    private Type _dispatcherInterfaceType = null!;
    private Assembly _pipelineAssembly = null!;
    private MethodInfo _dispatcherHandleMethod = null!;
    private readonly List<Type> _decoratorTypes = new();
    private Func<IServiceProvider, object> _dispatcherProxy = null!;
    private DispatcherOptions _dispatcherOptions = null!;

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IHandlerBuilder AddInput(Type type)
    {
        ProvidedTypeShouldBeInterface.Validate(type);

        _inputInterfaceType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type handlerType, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
        {
            throw new AssemblyNotProvidedException(nameof(AddHandler));
        }

        _handlerInterfaceType = handlerType;
        _handlerAssemblies = assemblies;
        ProvidedTypeShouldBeInterface.Validate(_handlerInterfaceType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputInterfaceType, _handlerInterfaceType);
        MethodShouldHaveAtLeastOneParameter.Validate(_handlerInterfaceType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputInterfaceType, _handlerInterfaceType);
        ValidateResultTypesWithHandlerGenericArguments.Validate(_handlerInterfaceType);

        _handlerHandleMethod = handlerType.GetFirstMethodInfo();

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>(Assembly pipelineAssembly) where TDispatcher : class
    {
        return AddDispatcher<TDispatcher>(new DispatcherOptions(), pipelineAssembly);
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>(DispatcherOptions options, Assembly pipelineAssembly)
        where TDispatcher : class
    {
        _dispatcherInterfaceType = typeof(TDispatcher);
        _pipelineAssembly = pipelineAssembly;

        ProvidedTypeShouldBeInterface.Validate(_dispatcherInterfaceType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputInterfaceType, _dispatcherInterfaceType);
        MethodShouldHaveAtLeastOneParameter.Validate(_dispatcherInterfaceType);
        ValidateInputTypeWithDispatcherMethodParameters.Validate(_inputInterfaceType, _dispatcherInterfaceType);
        ValidateResultTypesWithDispatcherInputResultTypes.Validate(_inputInterfaceType, _dispatcherInterfaceType);

        _dispatcherHandleMethod = _dispatcherInterfaceType.GetFirstMethodInfo();

        CrossValidateMethodParameters.Validate(_handlerInterfaceType, _dispatcherInterfaceType, _handlerHandleMethod,
            _dispatcherHandleMethod);
        CrossValidateResultTypes.Validate(_handlerInterfaceType, _dispatcherInterfaceType, _handlerHandleMethod,
            _dispatcherHandleMethod);

        _dispatcherProxy = provider =>
            DispatcherInterceptor.Create<TDispatcher>(provider, _handlerInterfaceType, _inputInterfaceType,
                _dispatcherOptions);

        _dispatcherOptions = options;

        return this;
    }

    public IPipelineDecoratorBuilder WithDecorator(Type genericDecorator)
    {
        return WithDecorator(new DecoratorOptions(), genericDecorator);
    }

    public IPipelineDecoratorBuilder WithDecorator<T>()
    {
        return WithDecorator<T>(new DecoratorOptions());
    }

    public IPipelineDecoratorBuilder WithDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies)
    {
        return WithDecorators(new DecoratorOptions(), action, assemblies);
    }

    public IPipelineDecoratorBuilder WithDecorator(DecoratorOptions decoratorOptions, Type genericDecorator)
    {
        if (decoratorOptions.StrictMode)
        {
            DecoratorValidator.Validate(genericDecorator, _handlerInterfaceType);
        }

        _decoratorTypes.Add(genericDecorator);
        return this;
    }

    public IPipelineDecoratorBuilder WithDecorator<T>(DecoratorOptions decoratorOptions)
    {
        var decoratorType = typeof(T);
        if (decoratorOptions.StrictMode)
        {
            DecoratorValidator.Validate(decoratorType, _handlerInterfaceType);
        }

        _decoratorTypes.Add(decoratorType);
        return this;
    }

    public IPipelineDecoratorBuilder WithDecorators(DecoratorOptions decoratorOptions,
        Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies)
    {
        if (!assemblies.Any())
        {
            throw new AssemblyNotProvidedException(nameof(WithDecorators));
        }

        var decorators = DecoratorsBuilder.BuildDecorators(action, _handlerInterfaceType, assemblies);

        if (decoratorOptions.StrictMode)
        {
            foreach (var decoratorType in decorators)
            {
                DecoratorValidator.Validate(decoratorType, _handlerInterfaceType);
            }
        }

        _decoratorTypes.AddRange(decorators);
        return this;
    }

    public void Build()
    {
        RegisterDispatcher();
        RegisterHandlersWithDecorators();
    }

    private void RegisterHandlersWithDecorators()
    {
        var handlers = AssemblyScanner.GetTypesBasedOnGenericType(_handlerAssemblies, _handlerInterfaceType)
            .WhereConstructorDoesNotHaveGenericParameter(_handlerInterfaceType)
            .ToList();

        _decoratorTypes.Reverse();

        _serviceCollection.AddHandlersWithDecorators(_decoratorTypes, handlers);
        _serviceCollection.AddHandlersRepository(_dispatcherInterfaceType, handlers);
    }

    private void RegisterDispatcher()
    {
        if (_dispatcherOptions.UseReflectionProxyImplementation)
        {
            RegisterProxyDispatcher();
        }
        else
        {
            RegisterGeneratedDispatcher();
        }
    }

    private void RegisterGeneratedDispatcher()
    {
        var dispatcherImplementations = _pipelineAssembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => TypeNamespaceComparer.CompareWithoutFullName(i, _dispatcherInterfaceType))).ToList();

        if (dispatcherImplementations.Count > 0)
        {
            foreach (var dispatcherImplementation in dispatcherImplementations)
            {
                _serviceCollection.AddSingleton(_dispatcherInterfaceType, dispatcherImplementation);
                _serviceCollection.AddSingleton<DispatcherGeneratorOptions>(x =>
                    new DispatcherGeneratorOptions(_dispatcherInterfaceType, _dispatcherOptions));
            }
        }
        else
        {
            throw new DispatcherNotRegisteredException(_dispatcherInterfaceType);
        }
    }

    private void RegisterProxyDispatcher()
    {
        _serviceCollection.AddSingleton(_dispatcherInterfaceType, _dispatcherProxy);
    }
}