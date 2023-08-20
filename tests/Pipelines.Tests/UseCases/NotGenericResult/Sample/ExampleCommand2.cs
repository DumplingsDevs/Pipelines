using Pipelines.Tests.UseCases.NotGenericResult.Types;

namespace Pipelines.Tests.UseCases.NotGenericResult.Sample;

public record ExampleCommand2(string Name, int Value) : IInput;