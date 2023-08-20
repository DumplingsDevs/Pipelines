using Pipelines.Tests.UseCases.ClassConstraintValidator.Types;

namespace Pipelines.Tests.UseCases.ClassConstraintValidator.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult>;