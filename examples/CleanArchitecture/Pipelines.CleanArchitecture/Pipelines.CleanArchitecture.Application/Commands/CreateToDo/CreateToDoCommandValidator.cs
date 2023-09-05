using Pipelines.CleanArchitecture.Abstractions.Commands;

namespace Pipelines.CleanArchitecture.Application.Commands.CreateToDo;

public class CreateToDoCommandValidator : ICommandValidator<CreateToDoCommand>
{
    public Task ValidateAsync(CreateToDoCommand command, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}