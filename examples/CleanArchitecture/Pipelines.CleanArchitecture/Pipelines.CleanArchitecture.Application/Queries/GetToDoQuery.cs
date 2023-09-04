using Pipelines.CleanArchitecture.Abstractions.Queries;

namespace Pipelines.CleanArchitecture.Application.Queries;

public record GetToDoQuery(Guid Id) : IQuery<GetToDoResult>;