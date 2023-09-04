using Pipelines.Tests.UseCases.HandlerWithStructResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithStructResult.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult>;