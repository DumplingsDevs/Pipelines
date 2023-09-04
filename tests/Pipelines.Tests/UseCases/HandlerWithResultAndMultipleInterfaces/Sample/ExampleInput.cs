using Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult>;