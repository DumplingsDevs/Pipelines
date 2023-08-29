using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;

public interface ICommandHandlerWithTwoResults<in TInput, TResult, TSecondResult>
    where TInput : ICommandWithTwoResults<TResult, TSecondResult> where TSecondResult : class
{
    public Task<(TResult, TSecondResult)> HandleAsync(TInput command, CancellationToken token);
}