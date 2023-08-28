namespace Pipelines.Tests.UseCases.TaskVoidHandler.Types;

public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}