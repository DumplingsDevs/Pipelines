using Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateResultTypesWithHandlerGenericArgument.Types.Handlers.Invalid;

public interface IReturnTwoValuesExpectedTwoTypeMismatch<in TCommand, TResult, TSecondResult> where TCommand : ICommandWithTwoResults<TResult,TSecondResult>
{
    public Task<(TResult,bool)> HandleAsync(TCommand command, CancellationToken token);
}