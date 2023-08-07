using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;

public record ExampleCommand(string Value) : ICommand<ExampleCommandResult>;