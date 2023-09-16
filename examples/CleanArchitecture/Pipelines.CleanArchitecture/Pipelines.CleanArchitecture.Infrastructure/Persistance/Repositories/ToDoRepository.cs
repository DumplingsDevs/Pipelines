using Microsoft.EntityFrameworkCore;
using Pipelines.CleanArchitecture.Domain;
using Pipelines.CleanArchitecture.Domain.Repositories;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance.Repositories;

internal class ToDoRepository : IToDoRepository
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

    public Task<ToDo?> GetAsync(Guid id, CancellationToken token)
    {
        return _context.ToDos.FirstOrDefaultAsync(x=> x.Id == id, token);
    }
}