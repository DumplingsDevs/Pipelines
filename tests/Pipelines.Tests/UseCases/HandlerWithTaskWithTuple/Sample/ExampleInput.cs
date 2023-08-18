using Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult, ExampleCommandResultSecond>;