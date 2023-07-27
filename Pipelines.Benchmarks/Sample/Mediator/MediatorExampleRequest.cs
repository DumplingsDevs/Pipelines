using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator;

public record MediatorExampleRequest(string Value) : IRequest<ExampleCommandResult>;