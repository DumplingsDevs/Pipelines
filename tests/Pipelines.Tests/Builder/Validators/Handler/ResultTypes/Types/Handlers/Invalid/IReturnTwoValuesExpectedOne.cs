using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnTwoValuesExpectedOne<in TCommand, TResult> where TCommand : ICommandWithResult<TResult>
{
    public Task<(TResult,bool)> HandleAsync(TCommand command, CancellationToken token);
}