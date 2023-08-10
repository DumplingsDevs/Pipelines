namespace Pipelines.Tests.UseCases.NotGenericResult.Types;

public interface ICommandDispatcher
{
    public Task<string> SendAsync(ICommand commandWithResult, CancellationToken token);
}