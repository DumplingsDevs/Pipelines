using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public record NotSentExampleRequest(string Value) : IRequest<ExampleCommandResult>;