namespace Pipelines.Tests.TestData;

public interface ICommandDispatcher
{
    public Task<string> SendAsync(ICommandWithResult commandWithResult);
}