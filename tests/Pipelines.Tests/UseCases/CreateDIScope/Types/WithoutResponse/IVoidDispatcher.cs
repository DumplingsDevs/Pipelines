namespace Pipelines.Tests.UseCases.CreateDIScope.Types.WithoutResponse;

public interface IVoidDispatcher
{
    public Task SendAsync(IVoidInput voidInput, CancellationToken token);
}