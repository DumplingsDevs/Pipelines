using Pipelines.Tests.UseCases.HandlerWithSingleParameter.Types;

namespace Pipelines.Tests.UseCases.HandlerWithSingleParameter;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<>);
    public Type GetHandlerType => typeof(ICommandHandler<,>);
}