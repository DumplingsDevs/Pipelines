namespace Pipelines.Tests.UseCases.SyncGenericResult.Sample;
using Types;

public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;