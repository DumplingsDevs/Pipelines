namespace Pipelines.Benchmarks.Types;

public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult>
{
    public Task<TResult> HandleAsync(TRequest request, CancellationToken token);
}