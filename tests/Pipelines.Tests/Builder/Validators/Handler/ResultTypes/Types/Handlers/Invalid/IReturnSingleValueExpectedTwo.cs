using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnSingleValueExpectedTwo<in TCommand, TResult, TSecondResult>
    where TCommand : ICommandWithTwoResults<TResult, TSecondResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}