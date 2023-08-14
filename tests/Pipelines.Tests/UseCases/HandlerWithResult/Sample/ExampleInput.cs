using Pipelines.Tests.UseCases.HandlerWithResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResult.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult>;