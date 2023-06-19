namespace Pipelines.Tests.TestData;

public record ExampleCommandWithResult(string Name, int Value) : ICommandWithResult;