namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface IHandlerWithResult<in TInput, TResult> where TInput : IInputWithResult<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}