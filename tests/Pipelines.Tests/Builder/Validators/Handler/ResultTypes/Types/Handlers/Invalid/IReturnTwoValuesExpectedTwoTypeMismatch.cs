using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnTwoValuesExpectedTwoTypeMismatch<in TCommand, TResult, TSecondResult>
    where TCommand : ICommandWithTwoResults<TResult, TSecondResult>
    where TResult : class
    where TSecondResult : class,IMarkerInterface
{
    public Task<(TSecondResult, TResult)> HandleAsync(TCommand command, CancellationToken token);
}