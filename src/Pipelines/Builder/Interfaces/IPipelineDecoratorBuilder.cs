using System.Reflection;
using Pipelines.Builder.Decorators;
using Pipelines.Contracts;

namespace Pipelines.Builder.Interfaces;

public interface IPipelineDecoratorBuilder : IPipelineBuildBuilder
{
    public IPipelineDecoratorBuilder WithOpenTypeDecorator(Type genericDecorator);
    IPipelineDecoratorBuilder WithClosedTypeDecorator<T>();

    IPipelineDecoratorBuilder WithClosedTypeDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies);

    public IPipelineDecoratorBuilder WithOpenTypeDecorator(DecoratorOptions decoratorOptions, Type genericDecorator);
    IPipelineDecoratorBuilder WithClosedTypeDecorator<T>(DecoratorOptions decoratorOptions);

    IPipelineDecoratorBuilder WithClosedTypeDecorators(DecoratorOptions decoratorOptions,
        Action<IPipelineClosedTypeDecoratorBuilder> action, params Assembly[] assemblies);
}