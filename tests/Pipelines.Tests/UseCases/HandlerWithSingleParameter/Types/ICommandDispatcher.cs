namespace Pipelines.Tests.UseCases.HandlerWithSingleParameter.Types;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command) where TResult : class;
}