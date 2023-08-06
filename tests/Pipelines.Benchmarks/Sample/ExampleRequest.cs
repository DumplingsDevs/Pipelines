using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample;

public record ExampleRequest(string Value) : IRequest<ExampleCommandResult>;