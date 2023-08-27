namespace Pipelines.Tests.SharedLibraryTest.Types;

public interface IDispatcherShared
{
    public Task<TResult> SendAsync<TResult>(IInputShared<TResult> input, CancellationToken token) where TResult : class;
}