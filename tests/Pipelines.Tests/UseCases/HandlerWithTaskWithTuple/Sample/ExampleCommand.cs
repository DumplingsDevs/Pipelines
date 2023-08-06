using Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult, ExampleCommandResultSecond>;