namespace Pipelines.Tests.UseCases.TaskVoidHandler.Types;

public interface IDispatcher
{
    public Task SendAsync(IInput input, CancellationToken token);
}