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
        new ExampleRequest1("My test request 1"),
        new ExampleRequest21("My test request 21"),
        new ExampleRequest41("My test request 41"),
        new ExampleRequest61("My test request 61"),
        new ExampleRequest81("My test request 81"),
        new ExampleRequest101("My test request 101"),
        new ExampleRequest121("My test request 121"),
        new ExampleRequest141("My test request 141"),
        new ExampleRequest161("My test request 161"),
        new ExampleRequest181("My test request 181"),
        new ExampleRequest201("My test request 201"),
        new ExampleRequest221("My test request 221"),
        new ExampleRequest241("My test request 241"),
        new ExampleRequest261("My test request 261"),
        new ExampleRequest281("My test request 281"),
        new ExampleRequest301("My test request 301"),
        new ExampleRequest321("My test request 321"),
        new ExampleRequest341("My test request 341"),
        new ExampleRequest361("My test request 361"),
        new ExampleRequest381("My test request 381"),
        new ExampleRequest401("My test request 401"),
        new ExampleRequest421("My test request 421"),
        new ExampleRequest441("My test request 441"),
        new ExampleRequest461("My test request 461"),
        new ExampleRequest481("My test request 481"),
    };


    private readonly List<MediatR.IRequest<ExampleCommandResult>> _mediatorRequests = new()
    {
        new MediatorExampleRequest1("My test request 1"),
        new MediatorExampleRequest21("My test request 21"),
        new MediatorExampleRequest41("My test request 41"),
        new MediatorExampleRequest61("My test request 61"),
        new MediatorExampleRequest81("My test request 81"),
        new MediatorExampleRequest101("My test request 101"),
        new MediatorExampleRequest121("My test request 121"),
        new MediatorExampleRequest141("My test request 141"),
        new MediatorExampleRequest161("My test request 161"),
        new MediatorExampleRequest181("My test request 181"),
        new MediatorExampleRequest201("My test request 201"),
        new MediatorExampleRequest221("My test request 221"),
        new MediatorExampleRequest241("My test request 241"),
        new MediatorExampleRequest261("My test request 261"),
        new MediatorExampleRequest281("My test request 281"),
        new MediatorExampleRequest301("My test request 301"),
        new MediatorExampleRequest321("My test request 321"),
        new MediatorExampleRequest341("My test request 341"),
        new MediatorExampleRequest361("My test request 361"),
        new MediatorExampleRequest381("My test request 381"),
        new MediatorExampleRequest401("My test request 401"),
        new MediatorExampleRequest421("My test request 421"),
        new MediatorExampleRequest441("My test request 441"),
        new MediatorExampleRequest461("My test request 461"),
        new MediatorExampleRequest481("My test request 481"),
    };


    [GlobalSetup(Targets = new string[] { nameof(WrapperDispatcherGenerator) })]
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

    [GlobalSetup(Target = nameof(WrapperDispatcherGeneratorWithDecorators))]
    public void SetupWithDecorators()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var servicesWithDecorators = new ServiceCollection();

        servicesWithDecorators.AddPipeline()
            .AddInput(typeof(Types.IRequest<>))
            .AddHandler((typeof(Types.IRequestHandler<,>)), assembly)
            .AddDispatcher<IRequestDispatcher>(new DispatcherOptions(false),
                assembly)
            .WithDecorator(typeof(ExampleRequestDecoratorOne<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorTwo<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorThree<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorFourUniqueNameForSearch<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorFive<,>))
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
            .WithDecorator(typeof(ExampleRequestDecoratorOne<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorTwo<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorThree<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorFourUniqueNameForSearch<,>))
            .WithDecorator(typeof(ExampleRequestDecoratorFive<,>))
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
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourOne<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourTwo<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourThree<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourFour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ExampleRequestBehaviourFive<,>));


        _mediatorWithBehavioursProvider = services.BuildServiceProvider();
    }

    [Benchmark]
    public async Task<List<ExampleCommandResult>> WrapperDispatcherGeneratorWithDecorators()
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
    public async Task<List<ExampleCommandResult>> WrapperDispatcherGenerator()
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
}