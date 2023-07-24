namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface.Samples;
using Types;

public class VoidDecorator: IVoidCommandHandler<VoidCommand>
{
    private readonly IVoidCommandHandler<VoidCommand> _commandHandler;

    public VoidDecorator(IVoidCommandHandler<VoidCommand> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task HandleAsync(VoidCommand command, CancellationToken token)
    {
        await _commandHandler.HandleAsync(command, token);
    }
}