using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;

public interface ICommandHandlerWithTwoResults<in TInput, TResult, TSecondResult>
    where TInput : IInputWithTwoResults<TResult, TSecondResult> where TResult : class where TSecondResult : class
{
    public Task<(TResult, TSecondResult)> HandleAsync(TInput command, CancellationToken token);
}