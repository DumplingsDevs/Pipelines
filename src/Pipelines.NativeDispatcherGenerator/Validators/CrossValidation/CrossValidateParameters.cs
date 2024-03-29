using System;
using Microsoft.CodeAnalysis;
using Pipelines.NativeDispatcherGenerator.Extensions;
using Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation.Exceptions;

namespace Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation;

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

        for (int i = 1; i < handlerMethod.Parameters.Length; i++)
        {
            var handlerParam = handlerMethod.Parameters[i];
            var dispatcherParam = dispatcherMethod.Parameters[i];

            if (!SymbolEqualityComparer.Default.Equals(handlerParam.Type, dispatcherParam.Type))
            {
                throw new ParameterTypeMismatchException(handlerMethod, dispatcherMethod, i);
            }
        }
    }
}
