using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Benchmarks.Types;

[GenerateImplementation]
public interface IRequestDispatcher
{
    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token);
}

class Example : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Example(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
    {
        var type = request.GetType();
        var handlerGeneric = typeof(IRequestHandler<,>);
        var requestAndResult = new[] { type, typeof(TResult) };
        var handlerType = handlerGeneric.MakeGenericType(requestAndResult);

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        return (TResult)await handler.HandleAsync((dynamic)request, token);
    }
}