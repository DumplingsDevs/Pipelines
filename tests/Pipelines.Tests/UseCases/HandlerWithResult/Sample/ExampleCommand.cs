using Pipelines.Tests.UseCases.HandlerWithResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResult.Sample;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult>;