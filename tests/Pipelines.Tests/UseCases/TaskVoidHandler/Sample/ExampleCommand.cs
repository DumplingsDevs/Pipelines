using Pipelines.Tests.UseCases.TaskVoidHandler.Types;

namespace Pipelines.Tests.UseCases.TaskVoidHandler.Sample;

public record ExampleCommand(string Value) : ICommand;