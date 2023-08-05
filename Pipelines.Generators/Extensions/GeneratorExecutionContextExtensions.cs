using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pipelines.Generators.Extensions;

internal static class GeneratorExecutionContextExtensions
{
    public static IEnumerable<ClassDeclarationSyntax> GetClassNodeByInterface(this GeneratorExecutionContext context, string interfaceName)
    {
        var syntaxTrees = context.Compilation.SyntaxTrees;

        foreach (var syntaxTree in syntaxTrees)
        {
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (root == null) continue;

            foreach (var classNode in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                // TO DO: Check also namespace
                if (classNode.HasInterface(interfaceName))
                {
                    yield return classNode;
                }
            }
        }
    }
    
    public static IEnumerable<TypeDeclarationSyntax> GetTypeNodes(this GeneratorExecutionContext context)
    {
        var syntaxTrees = context.Compilation.SyntaxTrees;

        foreach (var syntaxTree in syntaxTrees)
        {
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
            if (root == null) continue;

            foreach (var classNode in root.DescendantNodes().OfType<TypeDeclarationSyntax>())
            {
                yield return classNode;
            }
        }
    }
}