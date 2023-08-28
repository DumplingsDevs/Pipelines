namespace Pipelines.Tests.UseCases.InputWithInheritanceDispatcherWithGenericInput.Samples;
using Types;

public record ExampleInput(string Value) : IInput;