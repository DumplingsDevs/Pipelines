using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Pipelines.Generators.Extensions;

internal static class MethodSymbolExtensions
{
    public static bool IsAsync(this IMethodSymbol method)
    {
        // In theory we should check if something inside method uses await modifier, but this solution will work in our case
        return method.ReturnType.Name.Contains("Task");
    }

    public static List<string> GetTypeParametersConstraints(this IMethodSymbol method)
    {
        return method.TypeParameters.Select(GetTypeParameterConstraints).Where(s => !string.IsNullOrEmpty(s)).ToList();

        string GetTypeParameterConstraints(ITypeParameterSymbol typeParameter)
        {
            var constraints = new List<string>();
            if (typeParameter.HasReferenceTypeConstraint)
            {
                constraints.Add("class");
            }

            if (typeParameter.HasValueTypeConstraint)
            {
                constraints.Add("struct");
            }

            if (typeParameter.HasConstructorConstraint)
            {
                constraints.Add("new()");
            }
            
            constraints.AddRange(typeParameter.ConstraintTypes.Select(constraint => constraint.ToDisplayString()).ToList());

            //TO DO - other constraints ? Or maybe different approach

            if (constraints.Count == 0)
            {
                return string.Empty;
            }

            return $"{typeParameter.Name} : {string.Join(", ", constraints)}";
        }
    }
    
    public static List<ITypeSymbol> GetMethodGenericResults(this IMethodSymbol methodSymbol)
    {
        if (methodSymbol.ReturnsVoid)
        {
            return new List<ITypeSymbol>();
        }

        if (methodSymbol.ReturnType is ITypeParameterSymbol typeParameterSymbol)
        {
            return new List<ITypeSymbol>() { typeParameterSymbol };
        }

        var namedSymbol = ((INamedTypeSymbol)methodSymbol.ReturnType);

        if (namedSymbol.IsVoidTask())
        {
            return new List<ITypeSymbol>();
        }

        if (!namedSymbol.IsGenericType)
        {
            return new List<ITypeSymbol>();
        }

        var handlerResultTypeArguments = namedSymbol.TypeArguments.ToList();

        if (handlerResultTypeArguments.Count > 1)
        {
            return handlerResultTypeArguments;
        }

        if (handlerResultTypeArguments.Count == 0)
        {
            return new List<ITypeSymbol>();
        }

        switch (handlerResultTypeArguments.First())
        {
            case INamedTypeSymbol taskNamedResult:
            {
                if (!taskNamedResult.IsGenericType)
                {
                    return new List<ITypeSymbol>();
                }
                var taskArguments = taskNamedResult.TypeArguments;
                
                return taskArguments.Length > 0 ? taskArguments.ToList() : new List<ITypeSymbol> { taskNamedResult };
            }
            case ITypeParameterSymbol taskTypeResult:
                return new List<ITypeSymbol> { taskTypeResult };
        }

        return new List<ITypeSymbol>();
    }

    public static List<ITypeSymbol> GetMethodResults(this IMethodSymbol methodSymbol)
    {
        if (methodSymbol.ReturnsVoid)
        {
            return new List<ITypeSymbol>();
        }

        if (methodSymbol.ReturnType is ITypeParameterSymbol typeParameterSymbol)
        {
            return new List<ITypeSymbol>() { typeParameterSymbol };
        }

        var namedSymbol = ((INamedTypeSymbol)methodSymbol.ReturnType);

        if (namedSymbol.IsVoidTask())
        {
            return new List<ITypeSymbol>();
        }

        if (!namedSymbol.IsGenericType)
        {
            return new List<ITypeSymbol>() { namedSymbol };
        }

        var handlerResultTypeArguments = namedSymbol.TypeArguments.ToList();

        if (handlerResultTypeArguments.Count > 1)
        {
            return handlerResultTypeArguments;
        }

        if (handlerResultTypeArguments.Count == 0)
        {
            return new List<ITypeSymbol>();
        }

        switch (handlerResultTypeArguments.First())
        {
            case INamedTypeSymbol taskNamedResult:
            {
                var taskArguments = taskNamedResult.TypeArguments;

                return taskArguments.Length > 0 ? taskArguments.ToList() : new List<ITypeSymbol> { taskNamedResult };
            }
            case ITypeParameterSymbol taskTypeResult:
                return new List<ITypeSymbol> { taskTypeResult };
        }

        return new List<ITypeSymbol>();
    }
}