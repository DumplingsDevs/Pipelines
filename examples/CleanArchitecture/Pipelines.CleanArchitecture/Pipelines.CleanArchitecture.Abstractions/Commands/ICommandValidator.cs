namespace Pipelines.CleanArchitecture.Abstractions.Commands;

public interface ICommandValidator<in TCommand> where TCommand : ICommand
{
    Task ValidateAsync(TCommand command, CancellationToken token);
}