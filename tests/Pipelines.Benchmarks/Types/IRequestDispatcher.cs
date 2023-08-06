using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Sample;

namespace Pipelines.Benchmarks.Types;

public interface IRequestDispatcher
{
    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token) where TResult : class;
}