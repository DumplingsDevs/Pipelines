using Microsoft.CodeAnalysis;

namespace PipelinesGenerators;

internal static class NamedTypeSymbolExtensions
{
    public static string GetNameWithNamespace(this INamedTypeSymbol typeSymbol)
    {
        return $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}";
    }
    
    public static string GetFormattedFullname(this INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.GetNameWithNamespace().Replace(".", "");
    }
}
