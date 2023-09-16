namespace Pipelines.Tests.UseCases.HandlersNotFound.Types.WithotuResponse;

public interface IVoidDispatcher
{
    public Task SendAsync(IVoidInput voidInput, CancellationToken token);
}