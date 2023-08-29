using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnTwoValuesExpectedOne<in TInput, TResult>
    where TInput : ICommandWithResult<TResult>
{
    public Task<(TResult, bool)> HandleAsync(TInput command, CancellationToken token);
}