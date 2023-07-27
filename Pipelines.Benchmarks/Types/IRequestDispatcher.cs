namespace Pipelines.Benchmarks.Types;

public interface IRequestDispatcher
{
    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token);
}