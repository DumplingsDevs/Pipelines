namespace Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token)
        where TResult : class;
}