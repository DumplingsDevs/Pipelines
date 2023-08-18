using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult>;