using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Builder.Interfaces;
using Pipelines.Decorators;
using Pipelines.Utils;

namespace Pipelines.Builder;

public class PipelineBuilder : IInputBuilder, IHandlerBuilder, IDispatcherBuilder, IPipelineDecoratorBuilder,
    IPipelineBuildBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private Type _handlerType = null!;
    private Type _inputType = null!;
    private List<Type> _decorators = new();

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IHandlerBuilder AddInput(Type type)
    {
        _inputType = type;
        return this;
    }

    public IDispatcherBuilder AddHandler(Type type, Assembly assembly)
    {
        _handlerType = type;
        //quick workaround to filter out decorators
        var types = AssemblyScanner.GetTypesBasedOnGenericType(assembly, type).Where(x => !x.Name.Contains("Logging"));
        _serviceCollection.RegisterGenericTypesAsScoped(types);
        
        // foreach (var handlerType in types)
        // {
        //     var interfaces = handlerType.GetInterfaces();
        //     foreach (var @interface in interfaces)
        //     {
        //         if (@interface.IsGenericType)
        //         {
        //             _serviceCollection.AddScoped(@interface, sp =>
        //             {
        //                 var decoratorType = _decorators.First();
        //                 var resultType = _inputType.GetGenericArguments();
        //                 
        //                 var decoratorGeneric = decoratorType.MakeGenericType(_inputType, resultType.First());
        //                 
        //                 var handler = ActivatorUtilities.CreateInstance(sp, handlerType);
        //                 var decorator = ActivatorUtilities.CreateInstance(sp, decoratorType , new[] { handler });
        //                 return decorator;
        //             });
        //         }
        //     }
        // }

        return this;
    }

    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class
    {
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
        _decorators.AddRange(decorators);
        foreach (var decoratorType in decorators)
        {
            // _serviceCollection.AddScoped(decorator);
            // _serviceCollection.AddScoped(_decoratorType, decorator);


            int a = 0;
            for (var i = _serviceCollection.Count - 1; i >= 0; i--)
            {
                var serviceDescriptor = _serviceCollection[i];

                if (serviceDescriptor.ServiceType is DecoratedType)
                {
                    continue; // Service has already been decorated.
                }

                // if (!strategy.CanDecorate(serviceDescriptor.ServiceType))
                // {
                //     continue; // Unable to decorate using the specified strategy.
                // }

                if (!serviceDescriptor.ServiceType.Name.Contains("CommandHandler"))
                {
                    continue;
                }
                
                var decoratedType = new DecoratedType(serviceDescriptor.ServiceType);

                _serviceCollection.Add(serviceDescriptor.WithServiceType(decoratedType));

                _serviceCollection[i] = serviceDescriptor.WithImplementationFactory(CreateDecorator(decoratedType, decoratorType));
            }
        }

        return this;
    }
    
    private Func<IServiceProvider, object> CreateDecorator(Type serviceType, Type decoratorType)
    {

            var genericArguments = serviceType.GetGenericArguments();
            var closedDecorator = decoratorType.MakeGenericType(genericArguments);

            return TypeDecorator(serviceType, closedDecorator);

    }
    
    private static Func<IServiceProvider, object> TypeDecorator(Type serviceType, Type decoratorType) => serviceProvider =>
    {
        var instanceToDecorate = serviceProvider.GetRequiredService(serviceType);
        return ActivatorUtilities.CreateInstance(serviceProvider, decoratorType, instanceToDecorate);
    };
    
    public void Build()
    {
        //Dispatcher, Handler and Decorator implements method with same input / output parameters
        //Dispatcher, Handler and Decorator have same input type as provided in AddInput method
        //InputType shouldn't be object type
        //Only one method handle should be implemented in Dispatcher, Handler and Decorator
    }
}