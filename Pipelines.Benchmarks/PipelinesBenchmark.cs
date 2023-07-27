using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Sample;
using Pipelines.Benchmarks.Sample.Decorators;
using Pipelines.Benchmarks.Sample.Mediator;
using Pipelines.Benchmarks.Sample.Mediator.Behaviours;
using Pipelines.Benchmarks.Types;
using Pipelines.Public;

namespace Pipelines.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class PipelinesBenchmark
{
    private IServiceProvider _pipelinesProvider = null!;
    private IServiceProvider _pipelinesWithDecoratorsProvider = null!;
    private IServiceProvider _mediatorProvider = null!;
    private IServiceProvider _mediatorWithBehavioursProvider;

    [GlobalSetup(Target = nameof(Pipelines))]
    public void SetupPipelines()
    {
        var services = new ServiceCollection();

        services.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), Assembly.GetExecutingAssembly())
            .AddDispatcher<IRequestDispatcher>()
            .Build();
        
        _pipelinesProvider = services.BuildServiceProvider();
    }

    [GlobalSetup(Target = nameof(PipelinesWithDecorators))]
    public void SetupWithDecorators()
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var servicesWithDecorators = new ServiceCollection();
        servicesWithDecorators.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
    
        servicesWithDecorators.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), assembly)
            .AddDispatcher<IRequestDispatcher>()
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
    public async Task<ExampleCommandResult> PipelinesWithDecorators()
    {
        var dispatcher = _pipelinesWithDecoratorsProvider.GetRequiredService<IRequestDispatcher>();

        var request = new ExampleRequest("My test request");
    
        var result = await dispatcher.SendAsync(request, new CancellationToken());
    
        return result;
    }

    [Benchmark]
    public async Task<ExampleCommandResult> Pipelines()
    {
        var dispatcher = _pipelinesProvider.GetRequiredService<IRequestDispatcher>();
        
        var request = new ExampleRequest("My test request");
    
        var result = await dispatcher.SendAsync(request, new CancellationToken());
    
        return result;
    }

    [Benchmark(Baseline = true)]
    public async Task<ExampleCommandResult> MediatR()
    {
        var mediator = _mediatorProvider.GetRequiredService<IMediator>();
        
        var request = new MediatorExampleRequest("My test request");
    
        var result = await mediator.Send(request, new CancellationToken());
    
        return result;
    }
    
    [Benchmark()]
    public async Task<ExampleCommandResult> MediatRWithBehaviours()
    {
        var mediator = _mediatorWithBehavioursProvider.GetRequiredService<IMediator>();
        
        var request = new MediatorExampleRequest("My test request");
    
        var result = await mediator.Send(request, new CancellationToken());
    
        return result;
    }
}