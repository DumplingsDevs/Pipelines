using Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTupleResult;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<,>);
    public Type GetHandlerType => typeof(ICommandHandler<,,>);
}