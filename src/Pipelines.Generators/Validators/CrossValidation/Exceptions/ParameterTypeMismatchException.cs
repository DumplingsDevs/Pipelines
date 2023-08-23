using System;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Exceptions;

namespace Pipelines.Generators.Validators.CrossValidation.Exceptions;

public class ParameterTypeMismatchException : GeneratorException
{
    private static readonly string ErrorMessage = "The type of method parameter in the handler ({0}) does not match the type of parameter in the dispatcher ({1}) for parameter index {2}.";

    internal ParameterTypeMismatchException(IMethodSymbol handlerParamType, IMethodSymbol dispatcherParamType, int parameterIndex)
        : base(string.Format(ErrorMessage, handlerParamType, dispatcherParamType, parameterIndex))
    { }
}