using FluentValidation;

namespace Pipelines.CleanArchitecture.Application.Commands.CreateToDo;

public class CreateToDoCommandStructValidator : AbstractValidator<CreateToDoCommand>
{
    public CreateToDoCommandStructValidator()
    {
        RuleFor(x => x.Title).MinimumLength(2);
    }
}