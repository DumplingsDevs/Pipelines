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
    private IServiceProvider _pipelinesProvider = null!;
    private IServiceProvider _pipelinesWithDecoratorsProvider = null!;
    private IServiceProvider _pipelinesReflectionProvider = null!;
    private IServiceProvider _pipelinesReflectionWithDecoratorsProvider = null!;
    private IServiceProvider _mediatorProvider = null!;
    private IServiceProvider _mediatorWithBehavioursProvider = null!;
    private readonly List<Types.IRequest<ExampleCommandResult>> _pipelinesRequests = new()
    {
        new ExampleRequest("My test request"),
        new ExampleRequest10("My test request 10"),
        new ExampleRequest20("My test request 20"),
        new ExampleRequest30("My test request 30"),
        new ExampleRequest40("My test request 40"),
        new ExampleRequest50("My test request 50"),
        new ExampleRequest60("My test request 60"),
        new ExampleRequest70("My test request 70"),
        new ExampleRequest80("My test request 80"),
        new ExampleRequest90("My test request 90"),
        new ExampleRequest100("My test request 100"),
        new ExampleRequest110("My test request 110"),
        new ExampleRequest120("My test request 120"),
        new ExampleRequest130("My test request 130"),
        new ExampleRequest140("My test request 140"),
        new ExampleRequest150("My test request 150"),
        new ExampleRequest160("My test request 160"),
        new ExampleRequest170("My test request 170"),
        new ExampleRequest180("My test request 180"),
        new ExampleRequest190("My test request 190"),
        new ExampleRequest200("My test request 200"),
    };
    
    private readonly List<MediatR.IRequest<ExampleCommandResult>> _mediatorRequests = new()
    {
        new MediatorExampleRequest("My test request"),
        new MediatorExampleRequest10("My test request 10"),
        new MediatorExampleRequest20("My test request 20"),
        new MediatorExampleRequest30("My test request 30"),
        new MediatorExampleRequest40("My test request 40"),
        new MediatorExampleRequest50("My test request 50"),
        new MediatorExampleRequest60("My test request 60"),
        new MediatorExampleRequest70("My test request 70"),
        new MediatorExampleRequest80("My test request 80"),
        new MediatorExampleRequest90("My test request 90"),
        new MediatorExampleRequest100("My test request 100"),
        new MediatorExampleRequest110("My test request 110"),
        new MediatorExampleRequest120("My test request 120"),
        new MediatorExampleRequest130("My test request 130"),
        new MediatorExampleRequest140("My test request 140"),
        new MediatorExampleRequest150("My test request 150"),
        new MediatorExampleRequest160("My test request 160"),
        new MediatorExampleRequest170("My test request 170"),
        new MediatorExampleRequest180("My test request 180"),
        new MediatorExampleRequest190("My test request 190"),
        new MediatorExampleRequest200("My test request 200"),
    };

    [GlobalSetup(Targets = new string[] {nameof(Pipelines), nameof(DirectHandlerCall)})]
    public void SetupPipelines()
    {
        var services = new ServiceCollection();

        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), executingAssembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(false),
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
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(false),
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
    
    [GlobalSetup(Target = nameof(PipelinesReflection))]
    public void SetupPipelinesReflection()
    {
        var services = new ServiceCollection();

        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), executingAssembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(true),
                executingAssembly)
            .Build();
        _pipelinesReflectionProvider = services.BuildServiceProvider();
    }

    [GlobalSetup(Target = nameof(PipelinesReflectionWithDecorators))]
    public void SetupPipelinesReflectionWithDecorators()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var servicesWithDecorators = new ServiceCollection();

        servicesWithDecorators.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), assembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(true),
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
        _pipelinesReflectionWithDecoratorsProvider = servicesWithDecorators.BuildServiceProvider();
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
    
    [Benchmark]
    public async Task<List<ExampleCommandResult>> PipelinesReflectionWithDecorators()
    {
        var dispatcher = _pipelinesReflectionWithDecoratorsProvider.GetRequiredService<IRequestDispatcher>();

        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _pipelinesRequests)
        {
            var response = await dispatcher.SendAsync(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }

    [Benchmark]
    public async Task<List<ExampleCommandResult>> PipelinesReflection()
    {
        var dispatcher = _pipelinesReflectionProvider.GetRequiredService<IRequestDispatcher>();

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
        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _mediatorRequests)
        {
            using var scope = _mediatorProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var response = await mediator.Send(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }

    [Benchmark()]
    public async Task<List<ExampleCommandResult>> MediatRWithBehaviours()
    {
        var responses = new List<ExampleCommandResult>();
        
        foreach (var pipelinesRequest in _mediatorRequests)
        {
            using var scope = _mediatorWithBehavioursProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var response = await mediator.Send(pipelinesRequest, new CancellationToken());
            responses.Add(response);
        }

        return responses;
    }
    
    [Benchmark]
    public async Task<List<ExampleCommandResult>> DirectHandlerCall()
    {
        var handler = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest, ExampleCommandResult>>();
        var handler10 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest10, ExampleCommandResult>>();
        var handler20 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest20, ExampleCommandResult>>();
        var handler30 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest30, ExampleCommandResult>>();
        var handler40 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest40, ExampleCommandResult>>();
        var handler50 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest50, ExampleCommandResult>>();
        var handler60 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest60, ExampleCommandResult>>();
        var handler70 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest70, ExampleCommandResult>>();
        var handler80 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest80, ExampleCommandResult>>();
        var handler90 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest90, ExampleCommandResult>>();
        var handler100 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest100, ExampleCommandResult>>();
        var handler110 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest110, ExampleCommandResult>>();
        var handler120 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest120, ExampleCommandResult>>();
        var handler130 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest130, ExampleCommandResult>>();
        var handler140 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest140, ExampleCommandResult>>();
        var handler150 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest150, ExampleCommandResult>>();
        var handler160 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest160, ExampleCommandResult>>();
        var handler170 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest170, ExampleCommandResult>>();
        var handler180 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest180, ExampleCommandResult>>();
        var handler190 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest190, ExampleCommandResult>>();
        var handler200 = _pipelinesProvider.GetRequiredService<Types.IRequestHandler<ExampleRequest200, ExampleCommandResult>>();

        var responses = new List<ExampleCommandResult>();
        
        responses.Add(await handler.HandleAsync(new ExampleRequest("My test request"), new CancellationToken()));
        responses.Add(await handler10.HandleAsync(new ExampleRequest10("My test request 10"), new CancellationToken()));
        responses.Add(await handler20.HandleAsync(new ExampleRequest20("My test request 20"), new CancellationToken()));
        responses.Add(await handler30.HandleAsync(new ExampleRequest30("My test request 30"), new CancellationToken()));
        responses.Add(await handler40.HandleAsync(new ExampleRequest40("My test request 40"), new CancellationToken()));
        responses.Add(await handler50.HandleAsync(new ExampleRequest50("My test request 50"), new CancellationToken()));
        responses.Add(await handler60.HandleAsync(new ExampleRequest60("My test request 60"), new CancellationToken()));
        responses.Add(await handler70.HandleAsync(new ExampleRequest70("My test request 70"), new CancellationToken()));
        responses.Add(await handler80.HandleAsync(new ExampleRequest80("My test request 80"), new CancellationToken()));
        responses.Add(await handler90.HandleAsync(new ExampleRequest90("My test request 90"), new CancellationToken()));
        responses.Add(await handler100.HandleAsync(new ExampleRequest100("My test request 100"), new CancellationToken()));
        responses.Add(await handler110.HandleAsync(new ExampleRequest110("My test request 110"), new CancellationToken()));
        responses.Add(await handler120.HandleAsync(new ExampleRequest120("My test request 120"), new CancellationToken()));
        responses.Add(await handler130.HandleAsync(new ExampleRequest130("My test request 130"), new CancellationToken()));
        responses.Add(await handler140.HandleAsync(new ExampleRequest140("My test request 140"), new CancellationToken()));
        responses.Add(await handler150.HandleAsync(new ExampleRequest150("My test request 150"), new CancellationToken()));
        responses.Add(await handler160.HandleAsync(new ExampleRequest160("My test request 160"), new CancellationToken()));
        responses.Add(await handler170.HandleAsync(new ExampleRequest170("My test request 170"), new CancellationToken()));
        responses.Add(await handler180.HandleAsync(new ExampleRequest180("My test request 180"), new CancellationToken()));
        responses.Add(await handler190.HandleAsync(new ExampleRequest190("My test request 190"), new CancellationToken()));
        responses.Add(await handler200.HandleAsync(new ExampleRequest200("My test request 200"), new CancellationToken()));
        
        return responses;
    }
}