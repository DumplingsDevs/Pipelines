using Pipelines.Tests.Builder.Decorators.Types;

namespace Pipelines.Tests.Builder.Decorators.Samples;

public class FirstDecorator : ICommandHandler<DecoratedCommand, DecoratedCommandResult>, IDecoratorMarker
{
    private readonly ICommandHandler<DecoratedCommand, DecoratedCommandResult> _commandHandler;

    public FirstDecorator(ICommandHandler<DecoratedCommand, DecoratedCommandResult> commandHandler)
    {
        _commandHandler = commandHandler;
    }


    public async Task<DecoratedCommandResult> HandleAsync(DecoratedCommand command, CancellationToken token)
    {
        return await _commandHandler.HandleAsync(command, token);
    }
}