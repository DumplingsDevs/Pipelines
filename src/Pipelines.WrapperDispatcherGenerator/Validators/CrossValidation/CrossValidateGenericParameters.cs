using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation.Exceptions;

namespace Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation;

internal class CrossValidateGenericParameters
{
    internal static void Validate(IMethodSymbol dispatcherMethod, INamedTypeSymbol handlerType,
        INamedTypeSymbol inputType)
    {
        var typeArgumentsDispatcherCount = dispatcherMethod.TypeArguments.Length;
        var typeArgumentsHandlerCount = handlerType.TypeArguments.Skip(1).Count();
        var typeArgumentsInputCount = inputType.TypeArguments.Length;

        if (typeArgumentsInputCount > 0)
        {
            if (typeArgumentsDispatcherCount != typeArgumentsHandlerCount ||
                typeArgumentsHandlerCount != typeArgumentsInputCount)
            {
                throw new GenericArgumentsCountNotMatchException();
            }
        }
    }
}