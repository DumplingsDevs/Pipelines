using FluentValidation;

namespace Pipelines.CleanArchitecture.Application.Queries.GetToDo;

public class GetToDoStructValidator : AbstractValidator<GetToDoQuery>
{
    public GetToDoStructValidator()
    {
        RuleFor(x => x.Id).NotEqual(default(Guid));
    }
}