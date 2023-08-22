using Microsoft.CodeAnalysis;

namespace Pipelines.Generators.Extensions;

internal static class NamedTypeSymbolExtensions
{
    public static bool IsVoidTask(this INamedTypeSymbol symbol)
    {
        return symbol.ToString() == "System.Threading.Tasks.Task";
    }
}
