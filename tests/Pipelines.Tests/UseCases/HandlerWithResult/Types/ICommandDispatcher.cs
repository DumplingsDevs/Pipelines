namespace Pipelines.Tests.UseCases.HandlerWithResult.Types;

[GenerateImplementation]
public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token) where TResult : class;
}