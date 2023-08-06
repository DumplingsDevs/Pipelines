namespace Pipelines.Builder.Interfaces;

public interface IInputBuilder
{
    /// <summary>
    /// Adds a specific Input Interface Type to the pipeline builder. This interface will be implemented by Inputs.
    /// This is key for dynamically stitching different Input types, allowing system extendability and flexibility.
    /// </summary>
    /// <param name="type">This is the Type of the Input interface to be added to the pipeline builder. This method expects a interface Type that will be implemented by Inputs for Handlers</param>
    /// <returns>Returns an instance of IHandlerBuilder, allowing for fluent chaining of configuration methods.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions.ProvidedTypeIsNotInterfaceException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions.ReturnTypesShouldHaveClassConstraintException"></exception>
    public IHandlerBuilder AddInput(Type type);
}