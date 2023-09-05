using Pipelines.CleanArchitecture.Domain;
using Pipelines.CleanArchitecture.Domain.Repositories;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance.Repositories;

public class ToDoRepository : IToDoRepository
{
    private ToDoDbContext _context;

    public ToDoRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ToDo toDo, CancellationToken token)
    {
        await _context.AddAsync(toDo, token);
    }
}