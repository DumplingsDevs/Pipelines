using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public record DecoratorExampleDecoratorCommand(string Value) : IDecoratorCommand<DecoratorExampleCommandResult>;