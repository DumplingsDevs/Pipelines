namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Sample;
using Types;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult, int>;