namespace Pipelines.Builder.Interfaces;

public interface IPipelineDecoratorBuilder
{
    public void Build();
    public IPipelineBuildBuilder AddDecorators(Type decoratorGenericType, params Type[] decorators);
}