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
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException">The decorator's constructor does not have the required handler dependency (either it's missing or invalid). See the <a href="https://github.com/DumplingsDevs/Pipelines/blob/main/docs/troubleshooting.md#constructorvalidationexception">documentation</a> for troubleshooting details.</exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException">The decorator class is not implementing the expected interface, leading to a type mismatch between the expected and actual generic types. See the <a href="https://github.com/DumplingsDevs/Pipelines/blob/main/docs/troubleshooting.md#interfaceimplementationexception">documentation</a> for troubleshooting details.</exception>
    public IPipelineDecoratorBuilder WithDecorator(Type genericDecorator);

    /// <inheritdoc cref="WithDecorator"/>
    /// <param name="decoratorOptions">The Options for registering decorators</param>
    public IPipelineDecoratorBuilder WithDecorator(DecoratorOptions decoratorOptions, Type genericDecorator);

    /// <summary>
    /// Registers a closed type decorator to the pipeline.
    /// </summary>
    /// <typeparam name="T">The Type of the closed type decorator to add to the pipeline.</typeparam>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException">The decorator's constructor does not have the required handler dependency (either it's missing or invalid). See the <a href="https://github.com/DumplingsDevs/Pipelines/blob/main/docs/troubleshooting.md#constructorvalidationexception">documentation</a> for troubleshooting details.</exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException">The decorator class is not implementing the expected interface, leading to a type mismatch between the expected and actual generic types. See the <a href="https://github.com/DumplingsDevs/Pipelines/blob/main/docs/troubleshooting.md#interfaceimplementationexception">documentation</a> for troubleshooting details.</exception>
    IPipelineDecoratorBuilder WithDecorator<T>();
    
    /// <inheritdoc cref="WithDecorator{T}"/>
    /// <param name="decoratorOptions">The Options for registering decorators</param>
    IPipelineDecoratorBuilder WithDecorator<T>(DecoratorOptions decoratorOptions);

    /// <summary>
    /// Registers multiple closed type decorators to the pipeline.
    /// </summary>
    /// <param name="action">An Action that defines the builder for closed type decorators.</param>
    /// <param name="assemblies">An array of Assemblies where the decorators are located.</param>
    /// <returns>An IPipelineDecoratorBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.Constructor.Exceptions.ConstructorValidationException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions.InterfaceImplementationException"></exception>
    /// <exception cref="Pipelines.Exceptions.AssemblyNotProvidedException">At least one assembly is not provided to `AddHandler()` method. See the <a href="https://github.com/DumplingsDevs/Pipelines/blob/main/docs/troubleshooting.md#assemblynotprovidedexception">documentation</a> for troubleshooting details. </exception>
    IPipelineDecoratorBuilder WithDecorators(Action<IPipelineClosedTypeDecoratorBuilder> action,
        params Assembly[] assemblies);

    /// <inheritdoc cref="WithDecorators"/>
    /// <param name="decoratorOptions">The Options for registering decorators</param>
    IPipelineDecoratorBuilder WithDecorators(DecoratorOptions decoratorOptions,
        Action<IPipelineClosedTypeDecoratorBuilder> action, params Assembly[] assemblies);
}