using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pipelines.Builder.HandlerWrappers;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Exceptions;

public class PipelinesBenchmarksTypesIRequestDispatcherImplementation2 : Pipelines.Benchmarks.Types.IRequestDispatcher
{
    private abstract class PipelinesBenchmarksTypesIRequestDispatcherRequestHandlerBase
    {
        internal abstract Task<object> Handle(object request, CancellationToken token,
            IServiceProvider serviceProvider);
    }

    private class
        PipelinesBenchmarksTypesIRequestDispatcherRequestHandlerWrapperImpl<TRequest, TResult> :
            PipelinesBenchmarksTypesIRequestDispatcherRequestHandlerBase
        where TRequest : Pipelines.Benchmarks.Types.IRequest<TResult> where TResult : class

    {
        internal override async Task<object> Handle(object request, CancellationToken token,
            IServiceProvider serviceProvider) =>
            await Handle((Pipelines.Benchmarks.Types.IRequest<TResult>)request, serviceProvider, token)
                .ConfigureAwait(false);

        private async Task<TResult> Handle(Pipelines.Benchmarks.Types.IRequest<TResult> request,
            IServiceProvider serviceProvider,
            CancellationToken token)
        {
            var handler = serviceProvider
                .GetRequiredService<Pipelines.Benchmarks.Types.IRequestHandler<TRequest, TResult>>();

            return await handler.HandleAsync((TRequest)request, token);
        }
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, PipelinesBenchmarksTypesIRequestDispatcherRequestHandlerBase> _handlers = new();

    public PipelinesBenchmarksTypesIRequestDispatcherImplementation2(IServiceProvider serviceProvider,
        IHandlersRepository handlersRepository)
    {
        _serviceProvider = serviceProvider;
        var handlerTypes = handlersRepository.GetHandlers();
        foreach (var handlerType in handlerTypes)
        {
            var genericArguments = handlerType.GetInterfaces()
                .Single(i => i.GetGenericTypeDefinition() == typeof(Pipelines.Benchmarks.Types.IRequestHandler<,>))
                .GetGenericArguments();
            var requestType = genericArguments[0];
            var wrapperType =
                typeof(PipelinesBenchmarksTypesIRequestDispatcherRequestHandlerWrapperImpl<,>).MakeGenericType(
                    genericArguments);
            var wrapper = Activator.CreateInstance(wrapperType) ??
                          throw new InvalidOperationException($"Could not create wrapper type for {requestType}");
            _handlers[requestType] = (PipelinesBenchmarksTypesIRequestDispatcherRequestHandlerBase)wrapper;
        }
    }

    public async Task<TResult> SendAsync<TResult>(
        Pipelines.Benchmarks.Types.IRequest<TResult> request, CancellationToken token)
        where TResult : class
    {
        var requestType = request.GetType();
        if (!_handlers.TryGetValue(requestType, out var handlerWrapper))
        {
            throw new HandlerNotRegisteredException(requestType);
        }

        var result = await handlerWrapper.Handle(request, token, _serviceProvider);
        return (TResult)result;
        ;
    }
}