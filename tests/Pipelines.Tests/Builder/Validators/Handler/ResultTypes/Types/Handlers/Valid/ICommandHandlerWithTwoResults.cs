using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;

public interface ICommandHandlerWithTwoResults<in TCommand, TResult, TSecondResult>
    where TCommand : ICommandWithTwoResults<TResult, TSecondResult> where TResult : class where TSecondResult : class
{
    public Task<(TResult, TSecondResult)> HandleAsync(TCommand command, CancellationToken token);
}