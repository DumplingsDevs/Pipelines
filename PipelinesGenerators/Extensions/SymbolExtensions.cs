using Microsoft.CodeAnalysis;

namespace PipelinesGenerators.Extensions;

internal static class SymbolExtensions
{
    public static string GetNameWithNamespace(this ISymbol typeSymbol)
    {
        return $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}";
    }
    
    public static string GetFormattedFullname(this ISymbol typeSymbol)
    {
        return typeSymbol.GetNameWithNamespace().Replace(".", "");
    }
}
