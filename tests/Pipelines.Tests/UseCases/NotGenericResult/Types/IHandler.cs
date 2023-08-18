namespace Pipelines.Tests.UseCases.NotGenericResult.Types;

public interface IHandler<in TCommand> where TCommand : IInput
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}