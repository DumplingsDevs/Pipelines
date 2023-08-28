namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface IHandlerWithTwoResults<in TInput, TResult, TSecondResult> where TInput : IInputWithTwoResults<TResult, TSecondResult>
{
    public Task<(TResult,TSecondResult)> HandleAsync(TInput command, CancellationToken token);
}