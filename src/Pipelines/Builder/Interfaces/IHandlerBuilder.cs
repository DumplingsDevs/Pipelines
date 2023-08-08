using System.Reflection;

namespace Pipelines.Builder.Interfaces;

public interface IHandlerBuilder
{
    /// <summary>
    /// Adds a specific Handler Interface Type to the pipeline builder. This interface will be implemented by Handlers.
    /// </summary>
    /// <param name="handlerType">This is the Type of the Handler interface to be added to the pipeline builder. This method expects a interface Type that will be implemented by Handlers</param>
    /// <param name="assemblies">The Assemblies where the handlers are located</param>
    /// <returns>An IDispatcherBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions.ProvidedTypeIsNotInterfaceException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions.HandlerMethodNotFoundException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions.MultipleHandlerMethodsException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodWithOneParameter.Exceptions.MethodShouldHaveAtLeastOneParameterException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Handler.InputType.Exceptions.GenericArgumentsLengthMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Handler.InputType.Exceptions.GenericArgumentsNotFoundException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Handler.InputType.Exceptions.HandlerInputTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Handler.InputType.Exceptions.InvalidConstraintLengthException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ExpectedMethodWithResultException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ExpectedVoidMethodException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ResultTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions.ReturnTypesShouldHaveClassConstraintException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.GenericTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.GenericTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.IsGenericMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.TypeMismatchException"></exception>
    public IDispatcherBuilder AddHandler(Type handlerType, params Assembly[] assemblies);
}