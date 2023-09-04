namespace Pipelines.CleanArchitecture.Abstractions.Commands;

public interface ICommandDispatcher
{
    public Task<Guid?> HandleAsync(ICommand command, CancellationToken token);
}