using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler.Sample;

public record ExampleCommand(string Value) : ICommand;