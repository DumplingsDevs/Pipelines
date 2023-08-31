using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.NativeDispatcherGenerator.Extensions.Exceptions;

namespace Pipelines.NativeDispatcherGenerator.Extensions;

internal static class NamedTypeExtensions
{
    internal static IMethodSymbol GetFirstMethod(this INamedTypeSymbol typeSymbol)
    {
        var method = typeSymbol.ConstructedFrom.GetMembers().OfType<IMethodSymbol>().FirstOrDefault();

        if (method is null)
        {
            throw new MethodNotFoundException(typeSymbol);
        }

        return method!;
    }

    public static bool IsVoidTask(this INamedTypeSymbol symbol)
    {
        return symbol.ToString() == "System.Threading.Tasks.Task";
    }

    public static List<(ITypeParameterSymbol, string)> GetTypeParametersConstraints(this INamedTypeSymbol symbol)
    {
        return symbol.TypeParameters.Select(GetTypeParameterConstraints).Where(s => s.HasValue)
            .Select(x => (x.Value.Item1, x.Value.Item2)).ToList();

        (ITypeParameterSymbol, string)? GetTypeParameterConstraints(ITypeParameterSymbol typeParameter)
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

            constraints.AddRange(typeParameter.ConstraintTypes.Select(x => x.ToDisplayString())
                .ToList());

            //TO DO - other constraints ? Or maybe different approach

            if (constraints.Count == 0)
            {
                return null;
            }

            return (typeParameter, string.Join(", ", constraints));
        }
    }
    
    static string GenerateCommasForGenericParameters(int n)
    {
        if (n <= 0)
        {
            return "";
        }

        return string.Join("", Enumerable.Repeat(",", n));
    }

    private static string GetConstraintName(ITypeSymbol constraint)
    {
        if (constraint is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType)
            {
                return
                    $"{namedTypeSymbol.GetNameWithNamespace()}<{GenerateCommasForGenericParameters(namedTypeSymbol.TypeParameters.Length - 1)}>";
            }
        }
        return constraint.ToDisplayString();
    }
}