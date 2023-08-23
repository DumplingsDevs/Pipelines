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
        
        ValidateTypeSymbols(dispatcherMethod.ReturnType, handlerMethod.ReturnType);

        void ValidateTypeSymbols(ITypeSymbol typeSymbol1, ITypeSymbol typeSymbol2)
        {
            
            if (typeSymbol1.TypeKind != typeSymbol2.TypeKind)
            {
                ThrowMismatchException();
            }
            
            if (typeSymbol1 is INamedTypeSymbol dispatcherNamedType &&
                typeSymbol2 is INamedTypeSymbol handlerNamedType)
            {
                if (dispatcherNamedType.TypeArguments.Length != handlerNamedType.TypeArguments.Length)
                {
                    ThrowMismatchException();
                }

                for (var index = 0; index < dispatcherNamedType.TypeArguments.Length; index++)
                {
                    var dispatcherArgument = dispatcherNamedType.TypeArguments[index];
                    var handlerArgument = handlerNamedType.TypeArguments[index];

                    ValidateTypeSymbols(dispatcherArgument, handlerArgument);
                }
            }
        }

        void ThrowMismatchException()
        {
            throw new ReturnTypeMismatchException(dispatcherType, dispatcherMethod, handlerType, handlerMethod);
        }
    }
}