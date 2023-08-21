using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Extensions;
using Pipelines.Generators.Validators.CrossValidation.Exceptions;

namespace Pipelines.Generators.Validators.CrossValidation;

internal static class CrossValidateResultTypes
{
    internal static void Validate(INamedTypeSymbol dispatcherType, INamedTypeSymbol handlerType)
    {
        var dispatcherMethod = dispatcherType.GetFirstMethod();
        var handlerMethod = handlerType.GetFirstMethod();

        if (dispatcherMethod.ReturnType.ToString() != handlerMethod.ReturnType.ToString())
        {
            ThrowMismatchException();
        }

        void ThrowMismatchException()
        {
            throw new ReturnTypeMismatchException(dispatcherType, dispatcherMethod, handlerType, handlerMethod);
        }
    }
}