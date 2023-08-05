using Microsoft.Extensions.DependencyInjection;
using Pipelines.Tests.UseCases.HandlerWithMultipleResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<,>);
    public Type GetHandlerType => typeof(ICommandHandler<,,>);
}

internal class Test : ICommandDispatcher
{
    public (TResult, TResult2) SendAsync<TResult, TResult2>(ICommand<TResult, TResult2> command,
        CancellationToken token) where TResult : class where TResult2 : class
    {
        throw new NotImplementedException();
    }
}

public class PipelinesTestsUseCasesHandlerWithMultipleResultsTypesICommandDispatcherImplementation : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public PipelinesTestsUseCasesHandlerWithMultipleResultsTypesICommandDispatcherImplementation(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public (TResult, TResult2) SendAsync<TResult, TResult2>(ICommand<TResult, TResult2> command,
        CancellationToken token) where TResult : class where TResult2 : class
    {
        switch (command)
        {
            case Sample.ExampleCommand r:
                var result = _serviceProvider.GetRequiredService<ICommandHandler<Sample.ExampleCommand, Sample.ExampleRecordCommandResult, Sample.ExampleCommandClassResult>>().HandleAsync(r, token);
                return ((result.Item1 as TResult), (result.Item2 as TResult2));
            default: 
                throw new Exception();
        }
    }
}