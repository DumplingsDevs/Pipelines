using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class LoggingDecorator<TCommand, TResult> : IDecoratorCommandHandler<TCommand, TResult> where TCommand : IDecoratorCommand<TResult>
{
    private readonly IDecoratorCommandHandler<TCommand, TResult> _handler;

    public LoggingDecorator(IDecoratorCommandHandler<TCommand, TResult> handler)
    {
        _handler = handler;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken token)
    {
        Console.WriteLine($"Start of handling {command.GetType().Name}");
        var result = await _handler.HandleAsync(command, token);
        Console.WriteLine($"Stop of handling {command.GetType().Name}");

        return result;
    }
}