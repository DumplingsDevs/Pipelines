namespace Pipelines.Tests.UseCases.SyncGenericResult.Sample;
using Types;

public record ExampleInput(string Value) : IInput<ExampleCommandResult>;