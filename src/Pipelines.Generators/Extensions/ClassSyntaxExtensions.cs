using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pipelines.Generators.Extensions;

public static class ClassSyntaxExtensions
{
    /// <summary>Indicates whether or not the class has a specific interface.</summary>
    /// <returns>Whether or not the SyntaxList contains the attribute.</returns>
    public static bool HasInterface(this ClassDeclarationSyntax source, string interfaceName)
    {
        if (source.BaseList is null)
        {
            return false;
        }

        IEnumerable<BaseTypeSyntax> baseTypes = source.BaseList.Types.Select(baseType => baseType);
        // To Do - cleaner interface finding.
        return baseTypes.Any(baseType => baseType.ToString() == interfaceName);
    }

    public static INamedTypeSymbol? GetPropertyTypeSymbol(this ClassDeclarationSyntax classDeclarationSyntax,
        SemanticModel semanticModel, string propertyName)
    {
        var property = classDeclarationSyntax.DescendantNodes().OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault(p => p.Identifier.ValueText == propertyName);

        if (property != null)
        {
            var typeOfSyntax = property.DescendantNodes().OfType<TypeOfExpressionSyntax>()
                .FirstOrDefault();
            if (typeOfSyntax != null)
            {
                var identifierSyntax = typeOfSyntax.ChildNodes().OfType<IdentifierNameSyntax>()
                    .FirstOrDefault();

                if (identifierSyntax != null)
                {
                    var typeInfo = semanticModel.GetTypeInfo(identifierSyntax);
                    return (INamedTypeSymbol)typeInfo.Type!;
                }

                var generic = typeOfSyntax.ChildNodes().OfType<GenericNameSyntax>()
                    .FirstOrDefault();

                if (generic != null)
                {
                    var typeInfo = semanticModel.GetSymbolInfo(generic);
                    return ((INamedTypeSymbol)typeInfo.Symbol).ConstructedFrom;
                }
            }
        }

        return null;
    }
}