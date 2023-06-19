using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Valid;

public interface IVoidCommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}