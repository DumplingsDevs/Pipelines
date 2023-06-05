using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Pipelines;

public static class Extension
{
    public static PipelineBuilder AddPipeline(this IServiceCollection service)
    {
        return new PipelineBuilder(service);
    }
}

public class PipelineBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private readonly HandlerOptions _handlerTypes = new();

    public PipelineBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public PipelineBuilder AddDispatcher<TDispatcher>(string methodName) where TDispatcher : class
    {
        _serviceCollection.AddScoped<DispatcherInterceptor>();
        _serviceCollection.AddScoped<TDispatcher>(x =>
        {
            var interceptor = x.GetService<DispatcherInterceptor>();
            var proxyGenerator = new ProxyGenerator();
            return proxyGenerator.CreateInterfaceProxyWithoutTarget<TDispatcher>(interceptor);
        });

        return this;
    }

    // public PipelineBuilder AddHandler(Type type, Assembly assembly)
    // {
    //     _handlerTypes.Types.Add(type);
    //     
    //     //TBD: maybe we can remove scrutor and manually register
    //     _serviceCollection.Scan(s => s.FromAssemblies(assembly)
    //         .AddClasses(classes =>
    //             classes.AssignableTo(type).Where(_ => !_.IsGenericType))
    //         .AsImplementedInterfaces()
    //         .WithScopedLifetime());
    //
    //     return this;
    // }
    
    public PipelineBuilder AddHandler(Type type, Assembly assembly)
    {
        _handlerTypes.Types.Add(type);

        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type));

        foreach (var t in types)
        {
            var interfaces = t.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType)
                {
                    _serviceCollection.AddScoped(@interface, t);
                }
            }
        }

        return this;
    }

    public void Build()
    {
        _serviceCollection.AddSingleton(_handlerTypes);
    }
}


//ICommandDisptcher
//ICommandHandler
//ICommand
//ICommandResult

//IQueryDisptcher
//IQueryHandler
//IQuery
//IQuery

//LoggingPipelineBehaviour

/*
        services.AddPipeline(List<Assembly>)
            .AddDispatcher<ICommandDisptcher>(nameof(ICommandDisptcher))
            .AddHandler<ICommandHandler>(nameof(ICommandHandler.HandleAsync))
            .AddInput<ICommand>()
            .AddResult<ICommandResult>()
            .AddPipelineBehaviour<LoggingPipelineBehaviour>()
            .AddPipelineBehaviour<LoggingPipelineBehaviour>();

*/