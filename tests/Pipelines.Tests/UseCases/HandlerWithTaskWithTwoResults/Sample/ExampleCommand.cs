namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTwoResults.Sample;
using Types;
public record ExampleCommand(string Value) : ICommand<ExampleCommandResult, int>;