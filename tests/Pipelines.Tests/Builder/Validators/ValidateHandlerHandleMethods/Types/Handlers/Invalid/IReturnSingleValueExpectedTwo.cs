namespace Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Invalid;

using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.InputTypes;

public interface IReturnSingleValueExpectedTwo<in TCommand, TResult, TSecondResult>
    where TCommand : ICommandWithTwoResults<TResult, TSecondResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}