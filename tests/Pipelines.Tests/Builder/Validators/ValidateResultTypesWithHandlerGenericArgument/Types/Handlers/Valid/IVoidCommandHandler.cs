using Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.Handlers.Valid;

public interface IVoidCommandHandler<in TCommand> where TCommand : ICommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}