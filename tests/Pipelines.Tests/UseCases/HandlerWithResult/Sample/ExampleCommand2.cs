using Pipelines.Tests.UseCases.HandlerWithResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResult.Sample;

public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;