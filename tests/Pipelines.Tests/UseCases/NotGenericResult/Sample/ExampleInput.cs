using Pipelines.Tests.UseCases.NotGenericResult.Types;

namespace Pipelines.Tests.UseCases.NotGenericResult.Sample;

public record ExampleInput(string Name, int Value) : IInput;