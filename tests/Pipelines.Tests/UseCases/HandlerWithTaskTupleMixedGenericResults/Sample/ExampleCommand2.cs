using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Sample;

public record ExampleCommand2(string Value) : IInput<ExampleRecordCommandResult, ExampleCommandClassResult>;