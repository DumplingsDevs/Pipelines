namespace Pipelines.Tests.UseCases.CreateDIScope.Types.WithoutResponse;

public interface IVoidHandler<in TInput> where TInput : IVoidInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}