using Microsoft.EntityFrameworkCore;
using Pipelines.CleanArchitecture.Abstractions.Queries;
using Pipelines.CleanArchitecture.Application.Queries;
using Pipelines.CleanArchitecture.Application.Queries.GetToDo;
using Pipelines.CleanArchitecture.Infrastructure.Persistance;

namespace Pipelines.CleanArchitecture.Infrastructure.Queries.Handlers;

internal class GetToDoHandler : IQueryHandler<GetToDoQuery, GetToDoResult>
{
    private readonly ToDoDbContext _dbContext;

    public GetToDoHandler(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetToDoResult> HandleAsync(GetToDoQuery query, CancellationToken token)
    {
        var toDo = await _dbContext.ToDos.FirstAsync(cancellationToken: token);
        return new GetToDoResult(toDo.Id, toDo.Title);
    }
}