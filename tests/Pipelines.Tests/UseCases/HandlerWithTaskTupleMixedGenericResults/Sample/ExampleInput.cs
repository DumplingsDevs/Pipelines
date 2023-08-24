using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Sample;

public record ExampleInput(string Value) : IInput<ExampleRecordCommandResult, ExampleCommandClassResult>;