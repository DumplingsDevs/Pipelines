namespace Pipelines.Tests.UseCases.HandlerTwoHandleAsync.Sample;
using Types;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult>;