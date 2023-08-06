using Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Types;

namespace Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(ICommandDispatcher);
    public Type GetInputType => typeof(ICommand<>);
    public Type GetHandlerType => typeof(ICommandHandler<,>);
}