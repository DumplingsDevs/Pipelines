namespace Pipelines.Tests.UseCases.NotGenericResult.Types;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}