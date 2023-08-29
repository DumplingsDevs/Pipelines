using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnSingleValueExpectedTwo<in TInput, TResult, TSecondResult>
    where TInput : IInputWithTwoResults<TResult, TSecondResult> where TSecondResult : class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}