// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using BenchmarkDotNet.Running;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pipelines;
using Pipelines.Benchmarks;
using Pipelines.Benchmarks.Sample;
using Pipelines.Benchmarks.Sample.Mediator;
using Pipelines.Benchmarks.Types;

BenchmarkRunner.Run(typeof(Program).Assembly);

// var benchmark = new PipelinesBenchmark();
// benchmark.SetupPipelines();
// await benchmark.Pipelines();
// for (int i = 0; i < 10000; i++)
// {
//     
// }
// await ManualBenchmark();
// await ManualBenchmark();

async Task ManualBenchmark()
{
    var assembly = typeof(PipelinesBenchmark).Assembly;

    var services = new ServiceCollection();
    services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));

    services.AddPipeline()
        .AddInput(typeof(Pipelines.Benchmarks.Types.IRequest<>))
        .AddHandler((typeof(Pipelines.Benchmarks.Types.IRequestHandler<,>)), assembly)
        .AddDispatcher<IRequestDispatcher>()
        .Build();


    var provider = services.BuildServiceProvider();

    var mediatorRequest = new MediatorExampleRequest("My test request");
    var stopWatch = Stopwatch.StartNew();
    for (int i = 0; i < 1000000; i++)
    {
        var mediator = provider.GetRequiredService<IMediator>();
        await mediator.Send(mediatorRequest, new CancellationToken());
    }

    Console.Write("Mediator: ");
    Console.WriteLine(stopWatch.Elapsed);


    var pipelinesRequest = new ExampleRequest("My test request");
    stopWatch.Restart();
    for (int i = 0; i < 1000000; i++)
    {
        var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
        await dispatcher.SendAsync(pipelinesRequest, new CancellationToken());
    }

    Console.Write("Pipelines: ");
    Console.WriteLine(stopWatch.Elapsed);
}