using Microsoft.Extensions.DependencyInjection;
using Pipelines.Tests.UseCases.HandlerWithMultipleResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<,>);
    public Type GetHandlerType => typeof(ICommandHandler<,,>);
}
