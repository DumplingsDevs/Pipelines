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

        if (dispatcherMethod.ReturnType.TypeKind != handlerMethod.ReturnType.TypeKind)
        {
            ThrowMismatchException();
        }

        if (dispatcherMethod.ReturnType is INamedTypeSymbol dispatcherNamedType &&
            handlerMethod.ReturnType is INamedTypeSymbol handlerNamedType)
        {
            if (dispatcherNamedType.TypeArguments.Length != handlerNamedType.TypeArguments.Length)
            {
                ThrowMismatchException();
            }

            for (var index = 0; index < dispatcherNamedType.TypeArguments.Length; index++)
            {
                var dispatcherArgument = dispatcherNamedType.TypeArguments[index];
                var handlerArgument = handlerNamedType.TypeArguments[index];

                if (dispatcherArgument.TypeKind != handlerArgument.TypeKind)
                {
                    ThrowMismatchException();
                }

                if (dispatcherArgument.TypeKind != TypeKind.TypeParameter &&
                    handlerArgument.TypeKind != TypeKind.TypeParameter)
                {
                    if (dispatcherArgument.ToDisplayString() != handlerArgument.ToDisplayString())
                    {
                        ThrowMismatchException();
                    }
                }
            }
        }

        void ThrowMismatchException()
        {
            throw new ReturnTypeMismatchException(dispatcherType, dispatcherMethod, handlerType, handlerMethod);
        }
    }

    private static bool IsGenericReturnType(this IMethodSymbol typeSymbol)
    {
        return typeSymbol.ReturnType is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType;
    }

    private static bool IsTaskReturnType(this IMethodSymbol typeSymbol)
    {
        return typeSymbol.ReturnType is INamedTypeSymbol namedTypeSymbol &&
               namedTypeSymbol.OriginalDefinition.ToString() == "System.Threading.Tasks.Task<>" &&
               namedTypeSymbol.ContainingNamespace.ToString() == "System.Threading.Tasks";
    }
}