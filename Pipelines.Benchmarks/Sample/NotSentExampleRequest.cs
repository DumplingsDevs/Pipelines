using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample;

public record NotSentExampleRequest(string Value) : IRequest<ExampleCommandResult>;