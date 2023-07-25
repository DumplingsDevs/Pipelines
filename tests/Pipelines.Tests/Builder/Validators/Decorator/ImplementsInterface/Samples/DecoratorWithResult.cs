namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface.Samples;
using Types;

public class DecoratorWithResult : ICommandHandler<InputWithResult, Result>
{
    private readonly ICommandHandler<InputWithResult, Result> _commandHandler;

    public DecoratorWithResult(ICommandHandler<InputWithResult, Result> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<Result> HandleAsync(InputWithResult command, CancellationToken token)
    {
        return await _commandHandler.HandleAsync(command, token);
    }
}