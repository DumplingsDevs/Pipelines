namespace Pipelines.CleanArchitecture.Abstractions.Commands;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<Guid> HandleAsync(TCommand command, CancellationToken token);
}