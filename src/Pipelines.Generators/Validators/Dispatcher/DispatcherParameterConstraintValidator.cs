using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Extensions;
using Pipelines.Generators.Validators.Dispatcher.Exceptions;

namespace Pipelines.Generators.Validators.Dispatcher;

internal static class DispatcherParameterConstraintValidator
{
    internal static void Validate(INamedTypeSymbol dispatcherType)
    {
        var dispatcherMethod = dispatcherType.GetFirstMethod();
        
        if (!dispatcherMethod.TypeParameters.Any())
        {
            return;
        }

        if (dispatcherMethod.TypeParameters.Count(x => x.HasReferenceTypeConstraint) != dispatcherMethod.TypeParameters.Length)
        {
            throw new ConstraintOnTypeParameterNotFoundException(dispatcherMethod.TypeParameters.ToList());
        }
    }
}