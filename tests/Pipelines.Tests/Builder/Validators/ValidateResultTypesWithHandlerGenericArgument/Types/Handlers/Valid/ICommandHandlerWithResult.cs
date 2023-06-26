using Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.Handlers.Valid;

public interface ICommandHandlerWithResult<in TCommand, TResult> where TCommand : ICommandWithResult<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}