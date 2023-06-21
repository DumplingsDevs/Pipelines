namespace Pipelines.Builder.Interfaces;

public interface IPipelineDecoratorBuilder : IPipelineBuildBuilder
{
    public IPipelineBuildBuilder AddDecorators(params Type[] decorators);
}