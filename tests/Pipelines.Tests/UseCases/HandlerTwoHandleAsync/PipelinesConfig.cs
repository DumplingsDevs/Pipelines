using Pipelines.Tests.UseCases.HandlerTwoHandleAsync.Types;

namespace Pipelines.Tests.UseCases.HandlerTwoHandleAsync;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<>);
    public Type GetHandlerType => typeof(ICommandHandler<,>);
}