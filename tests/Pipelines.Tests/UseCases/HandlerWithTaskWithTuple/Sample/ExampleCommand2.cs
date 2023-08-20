using Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;

public record ExampleCommand2(string Value) : IInput<ExampleCommandResult, ExampleCommandResultSecond>;