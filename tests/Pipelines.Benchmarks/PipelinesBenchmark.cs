using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Sample;
using Pipelines.Benchmarks.Sample.Decorators;
using Pipelines.Benchmarks.Sample.Mediator;
using Pipelines.Benchmarks.Sample.Mediator.Behaviours;
using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class PipelinesBenchmark
{
    private const bool UseReflectionProxyImplementation = false;

    private IServiceProvider _pipelinesProvider = null!;
    private IServiceProvider _pipelinesWithDecoratorsProvider = null!;
    private IServiceProvider _mediatorProvider = null!;
    private IServiceProvider _mediatorWithBehavioursProvider = null!;
    private readonly List<Types.IRequest<ExampleCommandResult>> _pipelinesRequests = new()
    {
        // new ExampleRequest("My test request"),
        // new ExampleRequest20("My test request"),
        // new ExampleRequest40("My test request"),
        // new ExampleRequest60("My test request"),
        // new ExampleRequest80("My test request"),
        // new ExampleRequest100("My test request"),
        // new ExampleRequest120("My test request"),
        // new ExampleRequest140("My test request"),
        // new ExampleRequest160("My test request"),
        // new ExampleRequest180("My test request"),
        new ExampleRequest200("My test request"),
    };
    
    private readonly List<MediatR.IRequest<ExampleCommandResult>> _mediatorRequests = new()
    {
        // new MediatorExampleRequest("My test request"),
        // new MediatorExampleRequest20("My test request"),
        // new MediatorExampleRequest40("My test request"),
        // new MediatorExampleRequest60("My test request"),
        // new MediatorExampleRequest80("My test request"),
        // new MediatorExampleRequest100("My test request"),
        // new MediatorExampleRequest120("My test request"),
        // new MediatorExampleRequest140("My test request"),
        // new MediatorExampleRequest160("My test request"),
        // new MediatorExampleRequest180("My test request"),
        new MediatorExampleRequest200("My test request"),
    };

    [GlobalSetup(Target = nameof(Pipelines))]
    public void SetupPipelines()
    {
        var services = new ServiceCollection();

        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), executingAssembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(UseReflectionProxyImplementation),
                executingAssembly)
            .Build();
        _pipelinesProvider = services.BuildServiceProvider();
    }

    [GlobalSetup(Target = nameof(PipelinesWithDecorators))]
    public void SetupWithDecorators()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var servicesWithDecorators = new ServiceCollection();

        servicesWithDecorators.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), assembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(UseReflectionProxyImplementation),
                assembly)
            .WithOpenTypeDecorator(typeof(LoggingDecorator<,>))
            .WithOpenTypeDecorator(typeof(TracingDecorator<,>))
            .WithClosedTypeDecorators(x =>
            {
                x.WithImplementedInterface<IDecorator>();
                x.WithInheritedClass<BaseDecorator>();
                x.WithAttribute<DecoratorAttribute>();
                x.WithNameContaining("ExampleRequestDecoratorFourUniqueNameForSearch");
            }, assembly)
            .Build();
        servicesWithDecorators.AddSingleton<DecoratorsState>();
        _pipelinesWithDecoratorsProvider = servicesWithDecorators.BuildServiceProvider();
    }

    [GlobalSetup(Target = nameof(MediatR))]
    public void SetupMediator()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var services = new ServiceCollection();
        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));

        _mediatorProvider = services.BuildServiceProvider();
    }

    [GlobalSetup(Target = nameof(MediatRWithBehaviours))]
    public void SetupMediatorWithBehaviours()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var services = new ServiceCollection();
        services.AddSingleton<DecoratorsState>();
        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(TracingBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourOne<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourTwo<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourThree<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourFour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourFive<,>));


        _mediatorWithBehavioursProvider = services.BuildServiceProvider();
    }
    
    [Benchmark]
    public async Task<List<ExampleCommandResult>> PipelinesWithDecorators()
    {
        var dispatcher = _pipelinesWithDecoratorsProvider.GetRequiredService<IRequestDispatcher>();

        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _pipelinesRequests)
        {
            var response = await dispatcher.SendAsync(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }

    [Benchmark(Baseline = true)]
    public async Task<List<ExampleCommandResult>> Pipelines()
    {
        var dispatcher = _pipelinesProvider.GetRequiredService<IRequestDispatcher>();

        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _pipelinesRequests)
        {
            var response = await dispatcher.SendAsync(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }

    [Benchmark()]
    public async Task<List<ExampleCommandResult>> MediatR()
    {
        var mediator = _mediatorProvider.GetRequiredService<IMediator>();

        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _mediatorRequests)
        {
            var response = await mediator.Send(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }

    [Benchmark()]
    public async Task<List<ExampleCommandResult>> MediatRWithBehaviours()
    {
        var mediator = _mediatorWithBehavioursProvider.GetRequiredService<IMediator>();

        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _mediatorRequests)
        {
            var response = await mediator.Send(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }
}