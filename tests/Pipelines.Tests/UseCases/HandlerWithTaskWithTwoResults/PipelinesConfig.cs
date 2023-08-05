using Pipelines.Tests.UseCases.HandlerWithTaskWithTwoResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTwoResults;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<,>);
    public Type GetHandlerType => typeof(ICommandHandler<,,>);
}