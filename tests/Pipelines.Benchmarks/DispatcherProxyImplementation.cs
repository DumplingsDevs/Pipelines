using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Types;
using Pipelines.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Benchmarks;

internal class DispatcherProxyImplementation : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<Type, RequestHandlerBase> _handlers = new Dictionary<Type, RequestHandlerBase>();

    public DispatcherProxyImplementation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var handlerTypes = AssemblyScanner
            .GetTypesBasedOnGenericType(AppDomain.CurrentDomain.GetAssemblies(), typeof(IRequestHandler<,>))
            .WhereConstructorDoesNotHaveGenericParameter(typeof(IRequestHandler<,>));

        foreach (var handlerType in handlerTypes)
        {
            Type[] genericArguments = handlerType.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .GetGenericArguments();

            Type requestType = genericArguments[0];
            Type interfaceType = requestType.GetInterfaces().First();
            Type responseType = interfaceType.GetGenericArguments()[0];
            var wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, responseType);
            var wrapper = Activator.CreateInstance(wrapperType) ??
                          throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            _handlers[requestType] = (RequestHandlerBase)wrapper;
        }
    }


    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
        where TResult : class
    {
        Type requestType = request.GetType();

        if (!_handlers.TryGetValue(requestType, out var handlerWrapper))
        {
            throw new HandlerNotRegisteredException(requestType);
        }

        return await handlerWrapper.Handle(request, _serviceProvider, token) as TResult;
    }
}

public abstract class RequestHandlerBase
{
    public abstract Task<object> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

public class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerBase
    where TRequest : IRequest<TResponse> where TResponse : class
//TO DO: Probably TResponse don't need to be class constraint
{
    public override async Task<object> Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken) =>
        await Handle((IRequest<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

    public async Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        return await handler.HandleAsync((TRequest)request, cancellationToken);
    }
}