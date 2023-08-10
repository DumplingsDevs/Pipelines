namespace Pipelines.Tests.Models;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult>;