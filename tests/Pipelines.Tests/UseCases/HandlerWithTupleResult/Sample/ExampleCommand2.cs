using Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;

public record ExampleCommand2(string Value) : ICommand<ExampleRecordCommandResult, ExampleCommandClassResult>;