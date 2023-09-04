namespace Pipelines.CleanArchitecture.Abstractions.Commands;

public interface ICommandDispatcher
{
    public Task<Guid> SendAsync(ICommand command, CancellationToken token);
}