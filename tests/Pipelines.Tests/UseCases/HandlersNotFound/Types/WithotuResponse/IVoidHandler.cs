namespace Pipelines.Tests.UseCases.HandlersNotFound.Types.WithotuResponse;

public interface IVoidHandler<in TInput> where TInput : IVoidInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}