using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<>);
    public Type GetHandlerType => typeof(ICommandHandler<,>);
}