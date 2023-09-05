using Pipelines.CleanArchitecture.Abstractions.Queries;

namespace Pipelines.CleanArchitecture.Application.Queries.GetToDo;

public record GetToDoQuery(Guid Id) : IQuery<GetToDoResult>;