using System;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Extensions;
using Pipelines.Generators.Validators.CrossValidation.Exceptions;

namespace Pipelines.Generators.Validators.CrossValidation;

internal static class CrossValidateParameters
{
    internal static void Validate(INamedTypeSymbol dispatcherType, INamedTypeSymbol handlerType)
    {
        var dispatcherMethod = dispatcherType.GetFirstMethod();
        var handlerMethod = handlerType.GetFirstMethod();

        if (dispatcherMethod.Parameters.Length != handlerMethod.Parameters.Length)
        {
            throw new ParameterCountMismatchException(handlerMethod.Parameters.Length,
                dispatcherMethod.Parameters.Length);
        }

        for (int i = 0; i < handlerMethod.Parameters.Length; i++)
        {
            var handlerParam = handlerMethod.Parameters[i];
            var dispatcherParam = dispatcherMethod.Parameters[i];

            // if (!SymbolEqualityComparer.Default.Equals(handlerParam, dispatcherParam))
            // {
            //     throw new ParameterTypeMismatchException(handlerMethod, dispatcherMethod, i);
            // }
        }
    }
}
