namespace Pipelines.Tests.UseCases.HandlersNotFound.Types.WithotuResponseSync;

public interface ISyncVoidDispatcher
{
    public void SendAsync(ISyncVoidInput syncVoidInput);
}