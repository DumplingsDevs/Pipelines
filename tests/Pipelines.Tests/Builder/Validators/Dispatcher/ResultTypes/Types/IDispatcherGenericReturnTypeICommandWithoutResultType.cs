namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherGenericReturnTypeICommandWithoutResultType
{
    public Task<TResult> SendAsync<TResult>(ICommand request, CancellationToken token);
}