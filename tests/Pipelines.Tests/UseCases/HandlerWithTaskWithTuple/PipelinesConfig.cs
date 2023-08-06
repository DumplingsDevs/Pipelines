using Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<,>);
    public Type GetHandlerType => typeof(ICommandHandler<,,>);
}