namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface IDispatcher
{
    public void SendAsync(IInput input, CancellationToken token);
}