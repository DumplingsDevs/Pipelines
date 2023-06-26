using Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.Handlers.Invalid;

public interface IReturnSingleValueExpectedTwo<in TCommand, TResult, TSecondResult>
    where TCommand : ICommandWithTwoResults<TResult, TSecondResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}