using Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;

public record ExampleInput(string Value) : IInput<ExampleRecordCommandResult, ExampleCommandClassResult>;