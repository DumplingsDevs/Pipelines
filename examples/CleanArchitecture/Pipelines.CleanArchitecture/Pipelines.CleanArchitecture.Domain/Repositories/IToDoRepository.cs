namespace Pipelines.CleanArchitecture.Domain.Repositories;

public interface IToDoRepository
{
    public Task AddAsync(ToDo toDo, CancellationToken token);
    public Task<ToDo?> GetAsync(Guid id, CancellationToken token);
}