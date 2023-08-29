using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnTwoValuesExpectedTwoTypeMismatch<in TInput, TResult, TSecondResult>
    where TInput : IInputWithTwoResults<TResult, TSecondResult>
    where TSecondResult : IMarkerInterface
{
    public Task<(TSecondResult, TResult)> HandleAsync(TInput command, CancellationToken token);
}