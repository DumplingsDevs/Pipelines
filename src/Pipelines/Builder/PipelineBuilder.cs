using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Decorators;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Validators.CrossValidation.MethodParameters;
using Pipelines.Builder.Validators.CrossValidation.ResultType;
using Pipelines.Builder.Validators.Dispatcher.InputType;
using Pipelines.Builder.Validators.Dispatcher.ResultTypes;
using Pipelines.Builder.Validators.Handler.InputType;
using Pipelines.Builder.Validators.Handler.ResultTypes;
using Pipelines.Builder.Validators.Shared.InterfaceConstraint;
using Pipelines.Builder.Validators.Shared.MethodWithOneParameter;
using Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Assembly _handlerAssembly = null!;
    private Type _inputType = null!;
    private Type _dispatcherType = null!;
    private readonly List<Type> _decoratorTypes = new();
    private Func<IServiceProvider, object> _dispatcherProxy = null!;

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IHandlerBuilder AddInput(Type type)
    {
        ProvidedTypeShouldBeInterface.Validate(type);

        _inputType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type handlerType, Assembly assembly)
    {
        _handlerType = handlerType;
        _handlerAssembly = assembly;
        ProvidedTypeShouldBeInterface.Validate(_handlerType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType);
        MethodShouldHaveAtLeastOneParameter.Validate(_handlerType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputType, _handlerType);
        ValidateResultTypesWithHandlerGenericArguments.Validate(_handlerType);

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        _dispatcherType = typeof(TDispatcher);

        ProvidedTypeShouldBeInterface.Validate(_dispatcherType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _dispatcherType);
        MethodShouldHaveAtLeastOneParameter.Validate(_dispatcherType);
        ValidateInputTypeWithDispatcherMethodParameters.Validate(_inputType, _dispatcherType);
        ValidateResultTypesWithDispatcherInputResultTypes.Validate(_inputType, _dispatcherType);
        CrossValidateMethodParameters.Validate(_handlerType, _dispatcherType);
        CrossValidateResultTypes.Validate(_handlerType, _dispatcherType);
        
        _dispatcherProxy = provider => DispatcherInterceptor.Create<TDispatcher>(provider, _handlerType);

        return this;
    }

    public IPipelineDecoratorBuilder WithOpenTypeDecorator(Type genericDecorator)
    {
        // Validate if contains proper generic implementation
        // Validate if decorator's constructor has parameter with generic handler type 

        _decoratorTypes.Add(genericDecorator);
        return this;
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorator<T>()
    {
        // Validate if contains proper generic implementation
        // Validate if decorator's constructor has parameter with generic handler type 

        _decoratorTypes.Add(typeof(T));
        return this;
    }

    public IPipelineDecoratorBuilder WithClosedTypeDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies)
    {
        // Validate if contains proper generic implementation
        // Validate if decorator's constructor has parameter with generic handler type 

        var decorators = DecoratorsBuilder.BuildDecorators(action, _handlerType, assemblies);
        _decoratorTypes.AddRange(decorators);

        return this;
    }

    public void Build()
    {
        RegisterDispatcher();
        RegisterHandlerAndDecorators();
    }

    private void RegisterHandlerAndDecorators()
    {
        var handlers = AssemblyScanner.GetTypesBasedOnGenericType(_handlerAssembly, _handlerType)
            .WhereConstructorDoesNotHaveGenericParameter(_handlerType);

        _decoratorTypes.Reverse();

        _serviceCollection.AddDecorators(_decoratorTypes, handlers);
    }

    private void RegisterDispatcher()
    {
        _serviceCollection.AddScoped(_dispatcherType, _dispatcherProxy);
    }
}