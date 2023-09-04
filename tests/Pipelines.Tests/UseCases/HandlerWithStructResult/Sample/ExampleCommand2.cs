using Pipelines.Tests.UseCases.HandlerWithStructResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithStructResult.Sample;

public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;