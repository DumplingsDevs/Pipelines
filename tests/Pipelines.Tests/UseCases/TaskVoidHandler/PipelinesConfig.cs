using Pipelines.Tests.UseCases.TaskVoidHandler.Types;

namespace Pipelines.Tests.UseCases.TaskVoidHandler;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand);
    public Type GetHandlerType => typeof(ICommandHandler<>);
}