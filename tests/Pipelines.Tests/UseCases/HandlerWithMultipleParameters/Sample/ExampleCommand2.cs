using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;

public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;