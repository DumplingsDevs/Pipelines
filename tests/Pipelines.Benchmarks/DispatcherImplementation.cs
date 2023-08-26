using Microsoft.Extensions.DependencyInjection;
using Pipelines.Benchmarks.Sample;
using Pipelines.Benchmarks.Types;
using Pipelines.Exceptions;

namespace Pipelines.Benchmarks;

public interface IRequestHandlerFunc<TResult> where TResult : class
{
    Task<TResult> HandleAsync(IRequest<TResult> request, CancellationToken token);
}

internal class DispatcherImplementation
{
    private readonly IServiceProvider _serviceProvider;

    public DispatcherImplementation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        RegisterHandler<ExampleRequest, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest1, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest2, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest3, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest4, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest5, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest6, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest7, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest8, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest9, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest10, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest11, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest12, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest13, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest14, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest15, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest16, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest17, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest18, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest19, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest20, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest21, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest22, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest23, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest24, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest25, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest26, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest27, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest28, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest29, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest30, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest31, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest32, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest33, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest34, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest35, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest36, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest37, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest38, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest39, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest40, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest41, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest42, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest43, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest44, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest45, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest46, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest47, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest48, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest49, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest50, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest51, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest52, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest53, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest54, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest55, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest56, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest57, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest58, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest59, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest60, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest61, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest62, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest63, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest64, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest65, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest66, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest67, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest68, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest69, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest70, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest71, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest72, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest73, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest74, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest75, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest76, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest77, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest78, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest79, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest80, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest81, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest82, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest83, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest84, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest85, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest86, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest87, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest88, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest89, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest90, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest91, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest92, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest93, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest94, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest95, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest96, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest97, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest98, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest99, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest100, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest101, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest102, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest103, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest104, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest105, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest106, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest107, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest108, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest109, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest110, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest111, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest112, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest113, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest114, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest115, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest116, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest117, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest118, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest119, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest120, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest121, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest122, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest123, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest124, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest125, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest126, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest127, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest128, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest129, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest130, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest131, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest132, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest133, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest134, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest135, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest136, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest137, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest138, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest139, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest140, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest141, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest142, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest143, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest144, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest145, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest146, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest147, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest148, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest149, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest150, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest151, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest152, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest153, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest154, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest155, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest156, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest157, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest158, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest159, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest160, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest161, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest162, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest163, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest164, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest165, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest166, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest167, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest168, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest169, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest170, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest171, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest172, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest173, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest174, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest175, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest176, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest177, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest178, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest179, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest180, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest181, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest182, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest183, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest184, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest185, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest186, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest187, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest188, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest189, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest190, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest191, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest192, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest193, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest194, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest195, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest196, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest197, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest198, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest199, ExampleCommandResult>(serviceProvider);
        RegisterHandler<ExampleRequest200, ExampleCommandResult>(serviceProvider);
    }

    private readonly Dictionary<string, object> _handlers = new Dictionary<string, object>();

    private void RegisterHandler<TRequest, TResult>(IServiceProvider serviceProvider)
        where TRequest : IRequest<TResult>
        where TResult : class
    {
        _handlers[GetFullName(typeof(TRequest))] = new Func<IRequest<TResult>, CancellationToken, Task<TResult>>(
            async (request, token) =>
            {
                var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResult>>();
                if (handler == null)
                    throw new HandlerNotRegisteredException(typeof(IRequestHandler<TRequest, TResult>));

                return await handler.HandleAsync((TRequest)request, token);
            });
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
        where TResult : class
    {
        if (_handlers.TryGetValue(GetFullName(request.GetType()), out var handlerObj) &&
            handlerObj is Func<IRequest<TResult>, CancellationToken, Task<TResult>> handler)
        {
            return await handler(request, token);
        }

        throw new HandlerNotRegisteredException(request.GetType());
    }

    // public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token)
    //     where TResult : class
    // {
    //     Dictionary<Type, Func<IRequest<TResult>, Task<TResult>>> handlers = new()
    //     {
    //         {
    //             typeof(ExampleRequest), async request =>
    //             {
    //                 var resultPipelinesBenchmarksSampleExampleRequestHandler = _serviceProvider
    //                     .GetService<IRequestHandler<ExampleRequest, ExampleCommandResult>>();
    //                 if (resultPipelinesBenchmarksSampleExampleRequestHandler is null)
    //                     throw new HandlerNotRegisteredException(
    //                         typeof(IRequestHandler<ExampleRequest, ExampleCommandResult>));
    //                 var resultPipelinesBenchmarksSampleExampleRequest =
    //                     await resultPipelinesBenchmarksSampleExampleRequestHandler.HandleAsync((ExampleRequest)request,
    //                         token);
    //                 return resultPipelinesBenchmarksSampleExampleRequest as TResult;
    //             }
    //         }
    //     };
    //
    //     return await handlers[typeof(ExampleRequest)](request);
    // }

    private string GetFullName(Type type) => type.FullName ?? type.Namespace + type.Name;
}