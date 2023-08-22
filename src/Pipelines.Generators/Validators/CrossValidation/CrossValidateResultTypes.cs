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

        if (dispatcherMethod.ReturnsVoid != handlerMethod.ReturnsVoid)
        {
            ThrowMismatchException();
        }

        if (dispatcherMethod.IsGenericReturnType() != handlerMethod.IsGenericReturnType())
        {
            ThrowMismatchException();
        }

        if (dispatcherMethod.IsTaskReturnType() != handlerMethod.IsTaskReturnType())
        {
            ThrowMismatchException();
        }

        if (dispatcherMethod.ReturnType.IsValueType != handlerMethod.ReturnType.IsValueType)
        {
            ThrowMismatchException();
        }

        void ThrowMismatchException()
        {
            throw new ReturnTypeMismatchException(dispatcherType, dispatcherMethod, handlerType, handlerMethod);
        }
    }

    private static bool IsGenericReturnType(this IMethodSymbol typeSymbol)
    {
        return typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType;
    }

    private static bool IsTaskReturnType(this IMethodSymbol typeSymbol)
    {
        return typeSymbol is INamedTypeSymbol namedTypeSymbol &&
               namedTypeSymbol.OriginalDefinition.ToString() == "System.Threading.Tasks.Task<>" &&
               namedTypeSymbol.ContainingNamespace.ToString() == "System.Threading.Tasks";
    }
}