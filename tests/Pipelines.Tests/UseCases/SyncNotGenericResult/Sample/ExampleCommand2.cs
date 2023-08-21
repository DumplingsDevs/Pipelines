namespace Pipelines.Tests.UseCases.SyncNotGenericResult.Sample;
using Types;

public record ExampleCommand2(string Name, int Value) : IInput;