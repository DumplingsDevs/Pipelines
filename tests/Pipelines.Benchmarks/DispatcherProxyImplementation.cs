using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Types;
using Pipelines.Builder.HandlerWrappers;
using Pipelines.Exceptions;

namespace Pipelines.Benchmarks;

internal abstract class RequestHandlerBase
{
    internal abstract Task<object> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerBase
    where TRequest : IRequest<TResponse> where TResponse : class
//TO DO: Probably TResponse don't need to be class constraint
{
    internal override async Task<object> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken) =>
        await Handle((IRequest<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

    private async Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        return await handler.HandleAsync((TRequest)request, cancellationToken);
    }
}

internal class DispatcherProxyImplementation : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, RequestHandlerBase> _handlers = new();

    public DispatcherProxyImplementation(IServiceProvider serviceProvider, IHandlersRepository handlersRepository)
    {
        _serviceProvider = serviceProvider;

        var handlerTypes = handlersRepository.GetHandlers();

        foreach (var handlerType in handlerTypes)
        {
            var genericArguments = handlerType.GetInterfaces()
                .Single(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .GetGenericArguments();

            var requestType = genericArguments[0];
            var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(genericArguments);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper type for {requestType}");

            _handlers[requestType] = (RequestHandlerBase)wrapper;
        }
    }


    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
    {
        Type requestType = request.GetType();

        if (!_handlers.TryGetValue(requestType, out var handlerWrapper))
        {
            throw new HandlerNotRegisteredException(requestType);
        }

        return (TResult)await handlerWrapper.Handle(request, _serviceProvider, token);
    }
}