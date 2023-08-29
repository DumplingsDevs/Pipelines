using Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Sample;

public record ExampleInput(string Value) : IInput<string>;