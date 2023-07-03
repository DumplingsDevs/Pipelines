using Pipelines.Tests.Builder.Decorators.Types;

namespace Pipelines.Tests.Builder.Decorators.Samples;

public class ThirdDecorator : DecoratorBaseClass, ICommandHandler<DecoratedCommand, DecoratedCommandResult>
{
    private readonly ICommandHandler<DecoratedCommand, DecoratedCommandResult> _commandHandler;

    public ThirdDecorator(ICommandHandler<DecoratedCommand, DecoratedCommandResult> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<DecoratedCommandResult> HandleAsync(DecoratedCommand command, CancellationToken token)
    {
        return await _commandHandler.HandleAsync(command, token);
    }
}