using Pipelines.Tests.Builder.Decorators.Types;

namespace Pipelines.Tests.Builder.Decorators.Samples;

public class SecondDecorator02E6B297_F635_48D4_ABD9_D280AF6C3DB8 : ICommandHandler<DecoratedCommand, DecoratedCommandResult>
{
    private readonly ICommandHandler<DecoratedCommand, DecoratedCommandResult> _commandHandler;

    public SecondDecorator02E6B297_F635_48D4_ABD9_D280AF6C3DB8(ICommandHandler<DecoratedCommand, DecoratedCommandResult> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<DecoratedCommandResult> HandleAsync(DecoratedCommand command, CancellationToken token)
    {
        return await _commandHandler.HandleAsync(command, token);
    }
}