namespace Pipelines.Builder.Interfaces;

public interface IDispatcherBuilder
{
    /// <summary>
    /// Adds a specific Dispatcher Interface Type to the pipeline builder. This interface will be implemented by Dispatcher (which will be provided by Pipelines library).
    /// </summary>
    /// <typeparam name="TDispatcher">This is the Type of the Dispatcher interface to be added to the pipeline builder. This method expects a interface Type that will be implemented by Dispatcher.</typeparam>
    /// <returns>An IDispatcherBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions.ProvidedTypeIsNotInterfaceException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions.HandlerMethodNotFoundException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions.MultipleHandlerMethodsException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodWithOneParameter.Exceptions.MethodShouldHaveAtLeastOneParameterException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions.DispatcherMethodInputTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ExpectedMethodWithResultException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ExpectedVoidMethodException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ResultTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions.ReturnTypesShouldHaveClassConstraintException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.GenericTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.GenericTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.IsGenericMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.TypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions.ParameterCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions.ParameterTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions.ResultTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions.TaskReturnTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions.VoidAndValueMethodMismatchException"></exception>
    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class;
    
    /// <summary>
    /// Adds a specific Dispatcher Interface Type to the pipeline builder. This interface will be implemented by Dispatcher (which will be provided by Pipelines library).
    /// </summary>
    /// <typeparam name="TDispatcher">This is the Type of the Dispatcher interface to be added to the pipeline builder. This method expects a interface Type that will be implemented by Dispatcher.</typeparam>
    /// <param name="options">DispatcherOptions instance defining the options for dispatcher.</param>
    /// <returns>An IDispatcherBuilder instance that allows for further pipeline configuration.</returns>
    /// <exception cref="Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions.ProvidedTypeIsNotInterfaceException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions.HandlerMethodNotFoundException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.OnlyOneHandleMethod.Exceptions.MultipleHandlerMethodsException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodWithOneParameter.Exceptions.MethodShouldHaveAtLeastOneParameterException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Dispatcher.InputType.Exceptions.DispatcherMethodInputTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ExpectedMethodWithResultException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ExpectedVoidMethodException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions.ResultTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions.ReturnTypesShouldHaveClassConstraintException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.GenericTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.GenericTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.IsGenericMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions.TypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions.ParameterCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions.ParameterTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions.ResultTypeCountMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions.TaskReturnTypeMismatchException"></exception>
    /// <exception cref="Pipelines.Builder.Validators.CrossValidation.ResultType.Exceptions.VoidAndValueMethodMismatchException"></exception>
    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>(DispatcherOptions options) where TDispatcher : class;
}