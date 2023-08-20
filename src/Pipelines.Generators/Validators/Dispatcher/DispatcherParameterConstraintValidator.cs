using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Extensions;
using Pipelines.Generators.Validators.Dispatcher.Exceptions;

namespace Pipelines.Generators.Validators.Dispatcher;

internal static class DispatcherParameterConstraintValidator
{
    internal static void Validate(INamedTypeSymbol dispatcher)
    {
        var dispatcherMethods = dispatcher.GetMembers().OfType<IMethodSymbol>();
        foreach (var dispatcherMethod in dispatcherMethods)
        {
            if (!dispatcherMethod.TypeParameters.Any())
            {
                continue;
            }
            
            if (dispatcherMethod.GetTypeParametersConstraints().Count == 0)
            {
                throw new ConstraintOnTypeParameterNotFoundException(dispatcherMethod.TypeParameters.ToList());
            }
        }
    }
}