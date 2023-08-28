namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface ICommandHandlerWithTwoResults<in TInput, TResult, TSecondResult> where TInput : ICommandWithTwoResults<TResult, TSecondResult>
{
    public Task<(TResult,TSecondResult)> HandleAsync(TInput command, CancellationToken token);
}