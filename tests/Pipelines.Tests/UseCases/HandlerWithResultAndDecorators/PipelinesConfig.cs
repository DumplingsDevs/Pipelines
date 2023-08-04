using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators;

public class PipelinesConfig : IPipelineGeneratorConfig
{
    public Type GetDispatcherType => typeof(IRequestDispatcher);
    public Type GetInputType => typeof(IRequest<>);
    public Type GetHandlerType => typeof(IRequestHandler<,>);
}