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
            var constraints = typeParameter.ConstraintTypes.Select(constraint => constraint.ToDisplayString()).ToList();
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

            //TO DO - other constraints ? Or maybe different approach

            if (constraints.Count == 0)
            {
                return string.Empty;
            }

            return $"{typeParameter.Name} : {string.Join(", ", constraints)}";
        }
    }
}