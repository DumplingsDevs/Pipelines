using Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;

public record ExampleCommand(string Value) : ICommand<ExampleRecordCommandResult, ExampleCommandClassResult>;