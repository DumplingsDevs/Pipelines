using Microsoft.Extensions.DependencyInjection;
using Pipelines.Tests.UseCases.HandlerWithResult.Types;
using ExampleCommand = Pipelines.Tests.UseCases.HandlerWithResult.Sample.ExampleCommand;


namespace Pipelines.Tests.UseCases.HandlerWithResult;

public class DispatcherTestImplementation : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DispatcherTestImplementation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> request, CancellationToken token)
    {
        var type = request.GetType();
        var handlerGeneric = typeof(ICommandHandler<,>);
        var requestAndResult = new[] { type, typeof(TResult) };
        var handlerType = handlerGeneric.MakeGenericType(requestAndResult);
        
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        return (TResult)await handler.HandleAsync((dynamic)request, token);
    }
}