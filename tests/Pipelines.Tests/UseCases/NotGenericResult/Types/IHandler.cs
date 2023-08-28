namespace Pipelines.Tests.UseCases.NotGenericResult.Types;

public interface IHandler<in TInput> where TInput : IInput
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}