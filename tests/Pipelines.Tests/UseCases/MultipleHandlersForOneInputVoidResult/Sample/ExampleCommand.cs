namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Sample;
using Types;

public record ExampleCommand(string Value) : ICommand;