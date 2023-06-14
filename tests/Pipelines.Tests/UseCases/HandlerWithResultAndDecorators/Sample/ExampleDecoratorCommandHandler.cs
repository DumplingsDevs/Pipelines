using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class ExampleDecoratorCommandHandler : IDecoratorCommandHandler<DecoratorExampleDecoratorCommand, DecoratorExampleCommandResult>
{
    public Task<DecoratorExampleCommandResult> HandleAsync(DecoratorExampleDecoratorCommand decoratorCommand, CancellationToken token)
    {
        Console.WriteLine($"Handling {nameof(ExampleDecoratorCommandHandler)} in {nameof(ExampleDecoratorCommandHandler)}");
        return Task.FromResult(new DecoratorExampleCommandResult(decoratorCommand.Value + " Changed"));
    }
}