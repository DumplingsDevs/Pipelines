using System.Reflection;
using Pipelines.Builder.Decorators;

namespace Pipelines.Builder.Interfaces;

public interface IPipelineDecoratorBuilder : IPipelineBuildBuilder
{
    /// <summary>
    /// Registers an open type decorator to the pipeline.
    /// </summary>
    /// <param name="genericDecorator">The Type of the open type decorator to add to the pipeline.</param>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    public IPipelineDecoratorBuilder WithOpenTypeDecorator(Type genericDecorator);
    
    /// <summary>
    /// Registers an open type decorator to the pipeline.
    /// </summary>
    /// <param name="decoratorOptions">The Options for registering decorators</param>
    /// <param name="genericDecorator">The Type of the open type decorator to add to the pipeline.</param>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    public IPipelineDecoratorBuilder WithOpenTypeDecorator(DecoratorOptions decoratorOptions, Type genericDecorator);

    /// <summary>
    /// Registers a closed type decorator to the pipeline.
    /// </summary>
    /// <typeparam name="T">The Type of the closed type decorator to add to the pipeline.</typeparam>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    IPipelineDecoratorBuilder WithClosedTypeDecorator<T>();
    
    /// <summary>
    /// Registers a closed type decorator to the pipeline.
    /// </summary>
    /// <typeparam name="T">The Type of the closed type decorator to add to the pipeline.</typeparam>
    /// <param name="decoratorOptions">The Options for registering decorators</param>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    IPipelineDecoratorBuilder WithClosedTypeDecorator<T>(DecoratorOptions decoratorOptions);

    /// <summary>
    /// Registers multiple closed type decorators to the pipeline.
    /// </summary>
    /// <param name="action">An Action that defines the builder for closed type decorators.</param>
    /// <param name="assemblies">An array of Assemblies where the decorators are located.</param>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    IPipelineDecoratorBuilder WithClosedTypeDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies);

    /// <summary>
    /// Registers multiple closed type decorators to the pipeline.
    /// </summary>
    /// <param name="decoratorOptions">The Options for registering decorators</param>
    /// <param name="action">An Action that defines the builder for closed type decorators.</param>
    /// <param name="assemblies">An array of Assemblies where the decorators are located.</param>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    IPipelineDecoratorBuilder WithClosedTypeDecorators(DecoratorOptions decoratorOptions,
        Action<IPipelineClosedTypeDecoratorBuilder> action, params Assembly[] assemblies);
}