namespace Pipelines.Tests.Models;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult>;
public record ExampleCommand2(string Value) : ICommand<ExampleCommandResult>;