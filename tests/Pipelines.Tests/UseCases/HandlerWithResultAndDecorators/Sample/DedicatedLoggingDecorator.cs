using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class
    DedicatedLoggingDecorator : IDecoratorCommandHandler<DecoratorExampleDecoratorCommand,
        DecoratorExampleCommandResult>
{
    private readonly IDecoratorCommandHandler<DecoratorExampleDecoratorCommand, DecoratorExampleCommandResult> _handler;

    public DedicatedLoggingDecorator(
        IDecoratorCommandHandler<DecoratorExampleDecoratorCommand, DecoratorExampleCommandResult> handler)
    {
        _handler = handler;
    }

    public async Task<DecoratorExampleCommandResult> HandleAsync(DecoratorExampleDecoratorCommand command,
        CancellationToken token)
    {
        Console.WriteLine($"[DedicatedLoggingDecorator] Start of handling {command.GetType().Name}");
        var result = await _handler.HandleAsync(command, token);
        Console.WriteLine($"[DedicatedLoggingDecorator] Stop of handling {command.GetType().Name}");

        return result;
    }
}