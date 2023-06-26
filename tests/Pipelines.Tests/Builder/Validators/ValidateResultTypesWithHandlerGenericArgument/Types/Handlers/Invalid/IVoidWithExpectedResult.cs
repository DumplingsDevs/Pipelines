using Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.Handlers.Invalid;

public interface IVoidWithExpectedResult<in TCommand> where TCommand : ICommand
{
    public Task<int> HandleAsync(TCommand command, CancellationToken token);
}