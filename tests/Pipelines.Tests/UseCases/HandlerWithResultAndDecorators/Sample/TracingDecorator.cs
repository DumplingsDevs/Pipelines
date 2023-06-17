using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class TracingDecorator<TCommand, TResult> : IDecoratorCommandHandler<TCommand, TResult> where TCommand : IDecoratorCommand<TResult>
{
    private readonly IDecoratorCommandHandler<TCommand, TResult> _handler;

    public TracingDecorator(IDecoratorCommandHandler<TCommand, TResult> handler)
    {
        _handler = handler;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken token)
    {
        Console.WriteLine($"[TracingDecorator] Start of handling {command.GetType().Name}");
        var result = await _handler.HandleAsync(command, token);
        Console.WriteLine($"[TracingDecorator] Stop of handling {command.GetType().Name}");

        return result;
    }
}