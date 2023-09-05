namespace Pipelines.CleanArchitecture.Domain.Repositories;

public interface IToDoRepository
{
    public Task AddAsync(ToDo toDo, CancellationToken token);
}