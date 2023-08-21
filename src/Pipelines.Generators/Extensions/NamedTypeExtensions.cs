using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.Generators.Extensions.Exceptions;

namespace Pipelines.Generators.Extensions;

internal static class NamedTypeExtensions
{
    internal static IMethodSymbol GetFirstMethod(this INamedTypeSymbol typeSymbol)
    {
        var method = typeSymbol.GetMembers().OfType<IMethodSymbol>().FirstOrDefault();

        if (method is null)
        {
            throw new MethodNotFoundException(typeSymbol);
        }

        return method!;
    }
}
