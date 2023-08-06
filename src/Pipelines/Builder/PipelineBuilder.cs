using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Decorators;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Interfaces.Exceptions;
using Pipelines.Builder.Validators.CrossValidation.MethodParameters;
using Pipelines.Builder.Validators.CrossValidation.ResultType;
using Pipelines.Builder.Validators.Decorator;
using Pipelines.Builder.Validators.Dispatcher.InputType;
using Pipelines.Builder.Validators.Dispatcher.ResultTypes;
using Pipelines.Builder.Validators.Handler.InputType;
using Pipelines.Builder.Validators.Handler.ResultTypes;
using Pipelines.Builder.Validators.Input.ResultTypes;
using Pipelines.Builder.Validators.Shared.InterfaceConstraint;
using Pipelines.Builder.Validators.Shared.MethodWithOneParameter;
using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerInterfaceType = null!;
    private MethodInfo _handlerHandleMethod = null!;
    private Assembly _handlerAssembly = null!;
    private Type _inputInterfaceType = null!;
    private Type _dispatcherInterfaceType = null!;
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
        ValidateInputReturnTypes.Validate(type);

        _inputInterfaceType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type handlerType, Assembly assembly)
    {
        _handlerInterfaceType = handlerType;
        _handlerAssembly = assembly;
        ProvidedTypeShouldBeInterface.Validate(_handlerInterfaceType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputInterfaceType, _handlerInterfaceType);
        MethodShouldHaveAtLeastOneParameter.Validate(_handlerInterfaceType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputInterfaceType, _handlerInterfaceType);
        ValidateResultTypesWithHandlerGenericArguments.Validate(_handlerInterfaceType);

        _handlerHandleMethod = handlerType.GetFirstMethodInfo();

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        return AddDispatcher<TDispatcher>(new DispatcherOptions());
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>(DispatcherOptions options) where TDispatcher : class
    {
        _dispatcherInterfaceType = typeof(TDispatcher);

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
            DispatcherInterceptor.Create<TDispatcher>(provider, _handlerInterfaceType, _inputInterfaceType);

        _dispatcherOptions = options;

        return this;
    }

    public IPipelineDecoratorBuilder WithOpenTypeDecorator(Type genericDecorator)
    {
        return WithOpenTypeDecorator(new DecoratorOptions(), genericDecorator);
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorator<T>()
    {
        return WithClosedTypeDecorator<T>(new DecoratorOptions());
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies)
    {
        return WithClosedTypeDecorators(new DecoratorOptions(), action, assemblies);
    }

    public IPipelineDecoratorBuilder WithOpenTypeDecorator(DecoratorOptions decoratorOptions, Type genericDecorator)
    {
        if (decoratorOptions.StrictMode)
        {
            DecoratorValidator.Validate(genericDecorator, _handlerInterfaceType);
        }

        _decoratorTypes.Add(genericDecorator);
        return this;
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorator<T>(DecoratorOptions decoratorOptions)
    {
        var decoratorType = typeof(T);
        if (decoratorOptions.StrictMode)
        {
            DecoratorValidator.Validate(decoratorType, _handlerInterfaceType);
        }

        _decoratorTypes.Add(decoratorType);
        return this;
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorators(DecoratorOptions decoratorOptions,
        Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies)
    {
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
        var handlers = AssemblyScanner.GetTypesBasedOnGenericType(_handlerAssembly, _handlerInterfaceType)
            .WhereConstructorDoesNotHaveGenericParameter(_handlerInterfaceType);

        _decoratorTypes.Reverse();

        _serviceCollection.AddHandlersWithDecorators(_decoratorTypes, handlers);
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
        var dispatcherImplementations = _handlerAssembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => TypeNamespaceComparer.CompareWithoutFullName(i, _dispatcherInterfaceType))).ToList();

        if (dispatcherImplementations.Count > 0)
        {
            foreach (var dispatcherImplementation in dispatcherImplementations)
            {
                _serviceCollection.AddScoped(_dispatcherInterfaceType, dispatcherImplementation);
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