using Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Sample;

public record ExampleCommand2(string Value) : IInput<string>;