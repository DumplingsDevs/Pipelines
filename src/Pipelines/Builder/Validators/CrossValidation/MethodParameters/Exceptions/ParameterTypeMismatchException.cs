namespace Pipelines.Builder.Validators.CrossValidation.MethodParameters.Exceptions;

public class ParameterTypeMismatchException : Exception
{
    private static readonly string ErrorMessage = "The type of method parameter in the handler ({0}) does not match the type of parameter in the dispatcher ({1}) for parameter index {2}.";

    internal ParameterTypeMismatchException(Type handlerParamType, Type dispatcherParamType, int parameterIndex)
        : base(string.Format(ErrorMessage, handlerParamType, dispatcherParamType, parameterIndex))
    { }
}