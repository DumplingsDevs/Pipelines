namespace Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod.Types;

public interface IMultipleHandleMethod
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token);
    public Task<TResult> SendAsyncTheSecond<TResult>(ICommand<TResult> command, CancellationToken token);

}