using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Sample;

namespace Pipelines.Benchmarks.Types;

public interface IRequestDispatcher
{
    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token) where TResult : class;
}

class Example : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Example(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
        where TResult : class

    {
        switch (request)
        {
            case ExampleRequest r: return (await _serviceProvider.GetRequiredService<IRequestHandler<ExampleRequest, ExampleCommandResult>>().HandleAsync(r, token)) as TResult;
        }

        throw new Exception();
    }
}