namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

[GenerateImplementation]
public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken t, bool canDoSomething,
        Dictionary<string, string> dictionary) where TResult : class;
}