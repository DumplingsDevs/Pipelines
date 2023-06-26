using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Decorators;
using Pipelines.Builder.Interfaces;
using Pipelines.Builder.Validators;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private Type _dispatcherType = null!;
    private Type[] _decorators = null!;

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IHandlerBuilder AddInput(Type type)
    {
        _inputType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type handlerType, Assembly assembly)
    {
        _handlerType = handlerType;

        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, handlerType)
            .WhereConstructorDoesNotHaveParameter(handlerType);

        _serviceCollection.RegisterGenericTypesAsScoped(types);

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
        _dispatcherType = typeof(TDispatcher);

        _serviceCollection.AddScoped<DispatcherInterceptor>(x =>
            new DispatcherInterceptor(x, _inputType, _handlerType));
        _serviceCollection.AddScoped<TDispatcher>(x =>
        {
            var interceptor = x.GetService<DispatcherInterceptor>();
            var proxyGenerator = new ProxyGenerator();
            return proxyGenerator.CreateInterfaceProxyWithoutTarget<TDispatcher>(interceptor);
        });

        return this;
    }

    public IPipelineBuildBuilder AddDecorators(params Type[] decorators)
    {
        _decorators = decorators;

        return this;
    }

    public void Build()
    {
        AllProvidedTypesShouldBeInterface.Validate(_inputType, _handlerType, _dispatcherType);
        ExactlyOneHandleMethodShouldBeDefined.Validate(_inputType, _handlerType, _dispatcherType);
        ValidateInputTypeWithHandlerGenericArguments.Validate(_inputType, _handlerType);
        ValidateResultTypesWithHandlerGenericArguments.Validate(_handlerType);

        _serviceCollection.AddDecorators(_decorators);
    }
}