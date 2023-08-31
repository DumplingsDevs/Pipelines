using Pipelines.WrapperDispatcherGenerator.Exceptions;

namespace Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation.Exceptions;

public class ParameterCountMismatchException : GeneratorException
{
    private static readonly string ErrorMessage = "The number of method parameters in the handler ({0}) and dispatcher ({1}) are not the same.";

    internal ParameterCountMismatchException(int handlerParamCount, int dispatcherParamCount)
        : base(string.Format(ErrorMessage, handlerParamCount, dispatcherParamCount))
    { }
}