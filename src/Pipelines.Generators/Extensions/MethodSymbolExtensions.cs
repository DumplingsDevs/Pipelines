using Microsoft.CodeAnalysis;

namespace Pipelines.Generators.Extensions;

internal static class MethodSymbolExtensions
{
    public static bool IsAsync(this IMethodSymbol method)
    {
        // In theory we should check if something inside method uses await modifier, but this solution will work in our case
        return method.ReturnType.Name.Contains("Task");
    }
}
