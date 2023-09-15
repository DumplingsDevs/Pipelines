namespace Pipelines.Tests.UseCases.HandlersNotFound.Types.WithotuResponseSync;

public interface ISyncVoidHandler<in TInput> where TInput : ISyncVoidInput
{
    public void HandleAsync(TInput command);
}