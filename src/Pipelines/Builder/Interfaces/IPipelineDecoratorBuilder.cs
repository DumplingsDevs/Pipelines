using System.Reflection;

namespace Pipelines.Builder.Interfaces;

public interface IPipelineDecoratorBuilder : IPipelineBuildBuilder
{
    public IPipelineDecoratorBuilder WithOpenTypeDecorator(Type genericDecorator);
    IPipelineDecoratorBuilder WithClosedTypeDecorator<T>();
    IPipelineDecoratorBuilder WithClosedTypeDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action, params Assembly[] assemblies);
}