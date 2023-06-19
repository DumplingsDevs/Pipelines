namespace Pipelines.Tests.TestData;

public record ExampleCommand(string Name, int Value) : ICommand;