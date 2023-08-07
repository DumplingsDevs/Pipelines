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

    public static string GetParametersString(this IMethodSymbol method, int skip)
    {
        // In theory we should check if something inside method uses await modifier, but this solution will work in our case
        return string.Join(", ", method.Parameters.Skip(skip).Select(x => x.Name));
    }
}