using Pipelines.Tests.UseCases.NotGenericResult.Types;

namespace Pipelines.Tests.UseCases.NotGenericResult.Sample;

public record ExampleCommand(string Name, int Value) : ICommand;