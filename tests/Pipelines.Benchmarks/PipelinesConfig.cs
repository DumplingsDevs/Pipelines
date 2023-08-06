using BenchmarkDotNet.Configs;
using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(IRequestDispatcher);
    public Type GetInputType => typeof(IRequest<>);
    public Type GetHandlerType => typeof(IRequestHandler<,>);
}