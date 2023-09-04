using Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Sample;

public record ExampleCommand2(string Value) : IInput<ExampleCommandResult>;