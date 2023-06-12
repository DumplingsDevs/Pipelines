namespace Pipelines.Builder.Interfaces;

public interface IPipelineDecoratorBuilder : IPipelineBuildBuilder
{
    public IPipelineBuildBuilder AddDecorators(Type decoratorGenericType, params Type[] decorators);
}