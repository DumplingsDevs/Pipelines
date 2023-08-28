namespace Pipelines.Tests.UseCases.InputWithInheritanceDispatcherWithGenericInput.Samples;
using Types;

public record ExampleCommand2(string Value) : IInput;