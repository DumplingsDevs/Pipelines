namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult;
using Types;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand);
    public Type GetHandlerType => typeof(ICommandHandler<>);
}