namespace Pipelines.CleanArchitecture.Abstractions.Queries;

public interface IQueryDispatcher
{
    public Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken token);
}